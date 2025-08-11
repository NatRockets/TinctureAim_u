using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorPick : MonoBehaviour //arrow group showcase
{
    [SerializeField] private CutScreen targetObject;
    [SerializeField] private PickSlot[] slots;

    public void SetupPick(Action<PickSlot, bool> callback)
    {
        targetObject.Setup();
        
        foreach (var t in slots)
        {
            t.SetupSlot(callback, false);
        }
    }
    
    public void InitPick(Color target)
    {
        targetObject.Descale(true, 0.3f, () =>
        {
            Color.RGBToHSV(target, out var h, out float s, out float v);

            List<Color> picks = new List<Color>();
            picks.Add(target);
            
            float axisDir = 1f;
            for (int i = 0; i < slots.Length - 1; i++)
            {
                axisDir = -axisDir;
                picks.Add(Color.HSVToRGB((h + axisDir * Random.Range(0.2f, 0.3f)) % 1f, 1f, 1f));
            }
        
            Shuffle(picks);

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].SetSlot(picks[i]);
            }
            
            targetObject.Upscale(true);
        });
    }

    public void ResetPick()
    {
        targetObject.Descale();
    }
    
    private void Shuffle<T> (List<T> array)
    {
        Debug.Log("shuffling template array");
        
        int n = array.Count;
        while (n > 1) 
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}
