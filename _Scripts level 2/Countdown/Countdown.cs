using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown
{
    private float _currentTime = 0f;
    private float _duration = 0f;

    public Countdown(float duration)
    {
        _duration = duration;
        ResetTimer();
    }

    public void UpdateTimer()
    {
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        _currentTime = _duration;
    }

    public bool IsTimerElapsed()
    {
        return _currentTime <= 0f;
    }

    public float GetTimeRemaining()
    {
        return _currentTime;
    }
}
