using System.Collections;
using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class TargetGame : MonoBehaviour
{
    [SerializeField] private FingerCast cast;
    [SerializeField] private AimTarget aimTarget;
    [SerializeField] private FlyingArrow flyingArrow;
    [SerializeField] private int[] values;
    [SerializeField] private Text clearsUI;
    [SerializeField] private Text autosUI;
    [SerializeField] private Sprite clearsImage;
    [SerializeField] private Sprite autosImage;
    [SerializeField] private Sprite loseImage;
    [SerializeField] private CutScreen panel;
    [SerializeField] private Button restart;
    [SerializeField] private GameObject gameElems;

    private IntContainer data;

    private int hitId;

    private Vector3 hitPos;

    [Inject] private readonly Linker _linker;
    
    private void Awake()
    {
        panel.Setup();
        
        restart.onClick.AddListener(InitGame);
        
        aimTarget.Initialize(OnAccelerated, OnStopped, OnHit);
        cast.SetInputCallback(OnPositionPicked);
        flyingArrow.Initialize();
    }

    private void OnEnable()
    {
        if (data == null)
        {
            data = _linker.GetData();
        }
        
        panel.HideView();
        
        clearsUI.text = data.clears.ToString();
        autosUI.text = data.automoves.ToString();
        
        InitGame();
    }

    private void OnDisable()
    {
        DOTween.Kill("ui");
        aimTarget.StopRotating(true);
        StopAllCoroutines();
    }

    private void InitGame()
    {
        panel.HideView(true, () => gameElems.SetActive(true));
        flyingArrow.Hide(() =>
        {
            flyingArrow.ResetArrow();
            flyingArrow.SwitchCollider(true);
        });
        aimTarget.StartRotating();
        cast.SwitchInput(true);
    }

    private void OnAccelerated()
    {
        cast.SwitchInput(true);
    }

    private void OnStopped()
    {
        if (hitId > 0)
        {
            bool res = hitId > 1;
            if (res)
            {
                data.automoves++;
                autosUI.text = data.automoves.ToString();
            }
            else
            {
                data.clears++;
                clearsUI.text = data.clears.ToString();
            }
            StartCoroutine(TriggerFinish(res ? autosImage : clearsImage));
        }
        else
        {
            StartCoroutine(TriggerFinish(loseImage));
        }
        
        gameElems.SetActive(false);
    }
    
    private void OnPositionPicked(Vector3 pos)
    {
        cast.SwitchInput(false);
        hitPos = pos;
        flyingArrow.FlyTowards(pos, null);
    }

    private void OnHit()
    {
        aimTarget.StopRotating(false);
        float angle = aimTarget.CalculateAngle(hitPos);
        hitId = values[(int)(angle/(360f/values.Length))];
        _linker.PlaySound(2);
    }
    
    private IEnumerator TriggerFinish(Sprite win)
    {
        yield return new WaitForSeconds(1f);
        
        _linker.PlaySound(1);
        
        panel.SetView(win);
        panel.ShowView(true);
    }
}
