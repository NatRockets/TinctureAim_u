using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CutScreen : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private Image finalImage;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    [SerializeField] private Image shadeImage;

    private Transform panelTransform;
    private Vector3 panelInitPosition;

    public void Setup()
    {
        panelTransform = targetPanel.transform;
        panelInitPosition = panelTransform.position;
    }
    
    public void SetView(bool win)
    {
        finalImage.sprite = win ? winSprite : loseSprite;
        finalImage.SetNativeSize();
    }

    public void SetPos(float range)
    {
        panelTransform.position = new Vector3(panelInitPosition.x + Random.Range(-range, range), panelInitPosition.y + Random.Range(-range, range), panelInitPosition.z);
    }

    public void SetView(Sprite sprite)
    {
        finalImage.sprite = sprite;
        finalImage.SetNativeSize();
    }
    
    public void ShowView(bool animated = false, Action callback = null)
    {
        if (!animated)
        {
            targetPanel.SetActive(true);
            return;
        }
        
        panelTransform.localScale = Vector3.zero;
        targetPanel.SetActive(true);
        panelTransform.DOScale(1f, 0.3f)
            .SetId("ui")
            .OnKill(() =>
            {
                panelTransform.localScale = new Vector3(1f, 1f, 1f);
                if (shadeImage)
                {
                    shadeImage.enabled = true;;
                }
                callback?.Invoke();
            });
    }

    public void HideView(bool animated = false, Action callback = null)
    {
        if (!animated)
        {
            targetPanel.SetActive(false);
            return;
        }
        
        if (shadeImage)
        {
            shadeImage.enabled = false;
        }
        panelTransform.DOScale(0f, 0.3f)
            .SetId("ui")
            .OnKill(() =>
            {
                panelTransform.localScale = Vector3.zero;
                targetPanel.SetActive(false);
                callback?.Invoke();
            });
    }

    public void Descale(bool animated = false, float dur = 0.3f, Action callback = null)
    {
        if (!animated)
        {
            panelTransform.localScale = Vector3.zero;
            return;
        }
        
        panelTransform.DOScale(0f, dur)
            .SetId("ui")
            .OnKill(() =>
            {
                panelTransform.localScale = Vector3.zero;
                callback?.Invoke();
            });
    }

    public void Upscale(bool animated = false, Action callback = null)
    {
        if (!animated)
        {
            panelTransform.localScale = new Vector3(1f, 1f, 1f);
            return;
        }
        
        panelTransform.localScale = Vector3.zero;
        panelTransform.DOScale(1f, 0.3f)
            .SetId("ui")
            .OnKill(() =>
            {
                panelTransform.localScale = new Vector3(1f, 1f, 1f);
                callback?.Invoke();
            });
    }
}
