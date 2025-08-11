using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AimTarget : MonoBehaviour
{
    [SerializeField] private MonoWrapper monoWrapper;
    
    private Transform aTransform;
    private bool isRotating;

    private float coefficient;
    private float rotateSpeed;
    private float rotateSpeedTreeshold = 100f;
    private bool accel;
    
    private Action OnAccelerated;
    private Action OnDecelerated;
    
    private Action OnHit;
    
    private void RotateAim()
    {
        rotateSpeed += coefficient;
        if (!accel && rotateSpeed > rotateSpeedTreeshold)
        {
            accel = true;
            coefficient = 0;
            OnAccelerated?.Invoke();
        }
        
        transform.RotateAround(aTransform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        if (rotateSpeed <= 0)
        {
            isRotating = false;
            monoWrapper.UpdateEvent -= RotateAim;
            OnDecelerated?.Invoke();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FlyingArrow>(out var arrow))
        {
            arrow.SwitchCollider(false);
            arrow.KillAnim();
            arrow.SetParentTarget(aTransform);
            OnHit?.Invoke();
        }
    }
    public void Initialize(Action onAccelerated, Action onDecelerated, Action onHit)
    {
        aTransform = transform;
        OnAccelerated = onAccelerated;
        OnDecelerated = onDecelerated;
        OnHit = onHit;
    }

    public void StartRotating()
    {
        if (!isRotating)
        {
            rotateSpeedTreeshold = 100f * Random.Range(1f, 2f);
            rotateSpeed = 0f;
            coefficient = 0.4f;
            monoWrapper.UpdateEvent += RotateAim;
            isRotating = true;
            accel = false;
        }
    }

    public void StopRotating(bool instant)
    {
        if (instant)
        {
            monoWrapper.UpdateEvent -= RotateAim;
            isRotating = false;
        }
        else
        {
            coefficient = -0.5f;
        }
    }

    public float CalculateAngle(Vector3 point)
    {
        Vector3 hitDirection = point - aTransform.position;
        float signedAngle = Vector3.SignedAngle(hitDirection, aTransform.up, Vector3.forward);
        float angle360 = signedAngle;
        if (angle360 < 0)
        {
            angle360 += 360;
        }
        return 360f - angle360;
    }
}
