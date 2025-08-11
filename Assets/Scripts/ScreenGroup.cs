using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScreenGroup
{
    [SerializeField] private string nickname;
    [SerializeField] private Button[] triggers;
    [SerializeField] private List<GameObject> activeTargets;
    
    public void BindTriggers(Action<List<GameObject>> callback)
    {
        foreach (var trigger in triggers)
        {
            trigger.onClick.AddListener(() => callback(activeTargets));
        }
    }

    public List<GameObject> GetTargets()
    {
        return activeTargets;
    }

    public bool CheckName(string name)
    {
        return name.Equals(nickname);
    }
}
