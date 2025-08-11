using System;
using UnityEngine;

public class MonoWrapper : MonoBehaviour
{
    public event Action UpdateEvent;

    private void Update()
    {
        UpdateEvent?.Invoke();
    }
}
