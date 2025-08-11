using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float initialTime;
    [SerializeField] private Text timerUI;

    private float timeLeft;
    private IEnumerator timerCoroutine;

    private Action TimeoutCallback;

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
            TimeoutCallback?.Invoke();
        }
    }

    private void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Activate()
    {
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
    }

    public void Deactivate()
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    public void Refresh()
    {
        timeLeft = initialTime;
        UpdateTimer(timeLeft) ;
    }

    public void SetCallback(Action callback)
    {
        TimeoutCallback = callback;
    }
}
