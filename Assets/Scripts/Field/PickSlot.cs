using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickSlot : MonoBehaviour//arrow
{
    [SerializeField] private bool isUI;
    
    private EventTrigger trigger;
    private BoxCollider triggerCollider;
    
    private Button slotButton;
    private Color slotColor;
    private bool isFilled;

    private Image targetImage;
    private Material targetMaterial;
    private Action<PickSlot, bool> PickCallback;

    private Color defaultColor;

    public void SetupSlot(Action<PickSlot, bool> callback, bool isReserve)
    {
        defaultColor = Color.white;
        ColorUtility.TryParseHtmlString("#4D4747", out defaultColor);
        
        if (isUI)
        {
            slotButton = GetComponent<Button>();
            targetImage = transform.GetChild(0).GetComponent<Image>();
            
            if (isReserve)//no reserve option yet
            {
                return;
            }
            
            slotButton.onClick.AddListener(() => callback(this, isReserve));
        }
        else
        {
            MeshRenderer rend = transform.GetChild(0).GetComponent<MeshRenderer>();
            targetMaterial = Instantiate(rend.material);
            rend.material = targetMaterial;
            
            trigger = transform.GetComponent<EventTrigger>();
            triggerCollider = transform.GetComponent<BoxCollider>();
            
            PickCallback = callback;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { PickCell((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
    }

    private void PickCell(PointerEventData data)
    {
        PickCallback?.Invoke(this, false);
    }
    
    public bool IsFilled()
    {
        return isFilled;
    }

    public void SetSlot(Color target)
    {
        slotColor = target;
        if (isUI)
        {
            targetImage.color = target;
            slotButton.interactable = true;
        }
        else
        {
            targetMaterial.color = target;
            triggerCollider.enabled = true;
        }
        isFilled = true;
    }

    public void ResetSlot()
    {
        if (isUI)
        {
            targetImage.color = defaultColor;
            slotButton.interactable = false;
        }
        else
        {
            targetMaterial.color = defaultColor;
            triggerCollider.enabled = false;
        }
        isFilled = false;
    }

    public Color GetColor()
    {
        return slotColor;
    }
}
