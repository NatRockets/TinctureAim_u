using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScreens : MonoBehaviour
{
    [SerializeField] private RectTransform safeTransform;
    [Header("Initial group placed first!")]
    [SerializeField] private ScreenGroup[] groups;

    private List<GameObject> activeTargets;
    
    private void Awake()
    {
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.y /= Screen.height;

        safeTransform.anchorMin = anchorMin;
        safeTransform.anchorMax = anchorMax;

        foreach (var group in groups)
        {
            group.BindTriggers(SwitchTargets);
        }
    }

    private void Start()
    {
        activeTargets = groups[0].GetTargets();
        
        foreach (var target in activeTargets)
        {
            target.SetActive(true);
        }
    }

    private void SwitchTargets(List<GameObject> newTargets)
    {
        IEnumerable<GameObject> common = activeTargets.Intersect(newTargets);
        var commonObjects = common.ToList();
        
        foreach (var target in activeTargets)
        {
            if (!commonObjects.Contains(target))
            {
                target.SetActive(false);
            }
        }
        
        foreach (var target in newTargets)
        {
            if (!commonObjects.Contains(target))
            {
                target.SetActive(true);
            }
        }

        activeTargets = newTargets;
    }
}
