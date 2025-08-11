using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class FingerCast : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private MonoWrapper updateHandler;

    private System.Action<Vector3> InputCallback;
    
    private void OnDisable()
    {
        SwitchInput(false);
    }

    private void CheckHit(Vector3 inputPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.transform.parent.TryGetComponent<AimTarget>(out var aimTarget))
        {
            if(InputCallback != null)
            {
                InputCallback(hit.point);
            }
        }
    }

    private void LocalUpdate()
    {
#if !UNITY_EDITOR
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckHit(Input.GetTouch(0).position);
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            {
                CheckHit(Input.mousePosition);
            }
        }
#endif
    }

    public void SetInputCallback(System.Action<Vector3> callback)
    {
        InputCallback = callback;
    }

    public void SwitchInput(bool activate)
    {
        if (activate)
        {
            updateHandler.UpdateEvent += LocalUpdate;
        }
        else
        {
            updateHandler.UpdateEvent -= LocalUpdate;
        }
    }
}
