using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingArrow : MonoBehaviour
{
    private Transform targetT;
    private BoxCollider triggerCollider;
    private Material aMaterial;

    private Vector3 startPos;

    private Transform initParent;
    //private Transform tParent;

    public void Initialize()
    {
        targetT = transform;
        startPos = targetT.position;
        
        initParent = targetT.parent;
        
        MeshRenderer meshRenderer = targetT.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        aMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = aMaterial;
        
        triggerCollider = targetT.GetComponent<BoxCollider>();
    }
    
    public void SetColor(Color color)
    {
        aMaterial.color = color;
    }
    
    public void ResetArrow()
    {
        //tParent = null;
        targetT.parent = initParent;
        targetT.position = startPos;
        targetT.localScale = new Vector3(1,1,1);
    }

    public void FlyTowards(Vector3 target, Action callback)
    {
        targetT.position = startPos + new Vector3(Random.Range(-3f, 3f), Random.Range(-4f, 0f), 0f);
        targetT.forward = target - targetT.position;
        float randomZAngle = Random.Range(0f, 180f);
        targetT.Rotate(Vector3.forward, randomZAngle, Space.Self);
        
        targetT.DOMove(target, 0.5f)
            .SetId("obj")
            .OnKill(() =>
            {
                /*if (tParent != null)
                {
                    Debug.Log("set parent");
                    targetT.parent = tParent;
                }*/
                callback?.Invoke();
            });
    }

    public void Hide(Action callback)
    {
        targetT.DOScale(Vector3.zero, 0.4f)
            .OnKill(() => callback());
    }

    public void KillAnim()
    {
        DOTween.Kill("obj");
    }

    public void SwitchCollider(bool active)
    {
        triggerCollider.enabled = active;
    }

    public void SetParentTarget(Transform target)
    {
        targetT.parent = target;
    }
}
