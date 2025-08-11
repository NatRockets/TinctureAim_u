using System;
using UnityEngine;

public class ColorReserve : MonoBehaviour
{
    [SerializeField] private PickSlot[] slots;

    public void SetupReserve(Action<PickSlot, bool> callback)
    {
        foreach (var t in slots)
        {
            t.SetupSlot(callback, true);
        }
    }
    
    public void ResetColors()
    {
        foreach (var t in slots)
        {
            t.ResetSlot();
        }
    }

    public bool AddColor(Color target)
    {
        foreach (var t in slots)
        {
            if (!t.IsFilled())
            {
                t.SetSlot(target);
                return true;
            }
        }

        return false;
    }
}
