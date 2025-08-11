using System.Collections;
using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FieldGame : MonoBehaviour
{
    [SerializeField] private GameObject gameplayGroup;
    [SerializeField] private ColorCells cells;
    [SerializeField] private ColorReserve reserve;
    [SerializeField] private ColorPick pick;
    [SerializeField] private CutScreen cutScreen;
    [SerializeField] private HitMessage hitMessage;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button autoButton;
    [SerializeField] private Text clearsUI;
    [SerializeField] private Text autosUI;
    [SerializeField] private Timer timer;
    
    [SerializeField] private FlyingArrow flyingArrow;

    private bool executing;
    private Color currentTarget;

    private IntContainer data;
    
    [Inject] private readonly Linker _linker;
    
    private void Awake()
    {
        cutScreen.Setup();
        hitMessage.InitMessage();
        restartButton.onClick.AddListener(Restart);
        cells.Generate();
        reserve.SetupReserve(PickSlot);
        pick.SetupPick(PickSlot);
        
        clearButton.onClick.AddListener(Clear);
        autoButton.onClick.AddListener(DoAutomove);
        
        timer.SetCallback(DoTimeout);
        
        flyingArrow.Initialize();
    }

    private void OnEnable()
    {
        if (data == null)
        {
            data = _linker.GetData();
        }
        
        InitGame();

        CheckOptions();
        
        flyingArrow.ResetArrow();
    }

    private void OnDisable()
    {
        DOTween.Kill("ui");
        timer.Deactivate();
        StopAllCoroutines();
    }

    private void CheckOptions()
    {
        clearButton.interactable = data.clears > 0;
        autoButton.interactable = data.automoves > 0;
        clearsUI.text = data.clears.ToString();
        autosUI.text = data.automoves.ToString();
    }

    private void InitGame()
    {
        gameplayGroup.SetActive(true);
        timer.Deactivate();
        cutScreen.HideView(true);
        hitMessage.ResetMessage();
        cells.ResetCells();
        reserve.ResetColors();
        pick.ResetPick();
        
        InitRound();
    }

    private void InitRound()
    {
        flyingArrow.Hide(() =>
        {
            flyingArrow.ResetArrow();
        });
        
        currentTarget = cells.PickRandomUnfilled();
        pick.InitPick(currentTarget);
        executing = false;
        
        timer.Refresh();
        timer.Activate();
    }

    private void Restart()
    {
        InitGame();
    }

    private void DoTimeout()
    {
        if (executing)
        {
            return;
        }
        executing = true;
        
        _linker.PlaySound(2);

        if (reserve.AddColor(currentTarget))
        {
            InitRound();
        }
        else
        {
            PlayFinish(false);
        }
    }

    private void PickSlot(PickSlot slot, bool isReserve)
    {
        if (executing)
        {
            return;
        }

        executing = true;
        
        timer.Deactivate();
        
        flyingArrow.SetColor(slot.GetColor());
        flyingArrow.FlyTowards(cells.GetCurrentCellPosition(), () =>
        {
            if (CheckColor(slot.GetColor()))
            {
                _linker.PlaySound(1);
                if (isReserve)
                {
                    slot.ResetSlot();
                }
            
                if (cells.SetCurrentCell(currentTarget))
                {
                    PlayFinish(true);
                }
                else
                {
                    hitMessage.PlayHit();
                    InitRound();
                }
                return;
            }
            
            _linker.PlaySound(2);
            if (isReserve)
            {
                return;
            }

            if (reserve.AddColor(slot.GetColor()))
            {
                hitMessage.PlayMiss();
                InitRound();
            }
            else
            {
                PlayFinish(false);
            }
        });
    }

    private bool CheckColor(Color temp)
    {
        //Debug.Log($"{temp.r}, {temp.g}, {temp.b}");
        float deltaR = currentTarget.r - temp.r;
        float deltaG = currentTarget.g - temp.g;
        float deltaB = currentTarget.b - temp.b;
        return (Mathf.Abs(deltaR) < 0.01) && (Mathf.Abs(deltaG) < 0.01) && (Mathf.Abs(deltaB) < 0.01);
    }

    private void DoAutomove()
    {
        if (executing)
        {
            return;
        }
        
        timer.Deactivate();
        
        flyingArrow.SetColor(currentTarget);
        flyingArrow.FlyTowards(cells.GetCurrentCellPosition(), () =>
        {
            _linker.PlaySound(1);
            if (cells.SetCurrentCell(currentTarget))
            {
                PlayFinish(true);
            }
            else
            {
                InitRound();
            }

            data.automoves--;
            CheckOptions();
        });
    }

    private void Clear()
    {
        if (executing)
        {
            return;
        }

        reserve.ResetColors();
        data.clears--;
        CheckOptions();
    }
    
    private void PlayFinish(bool win)
    {
        gameplayGroup.SetActive(false);
        StartCoroutine(TriggerFinish(win));
    }

    private IEnumerator TriggerFinish(bool win)
    {
        yield return new WaitForSeconds(1f);
        
        cutScreen.SetView(win);
        cutScreen.ShowView(true);
    }
}
