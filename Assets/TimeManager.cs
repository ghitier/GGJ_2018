using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager> {

    public float totalTime = 300f;

    private static float _startTime;
    private bool _running = true;

    public static float ElapsedTime
    {
        get
        {
            return Time.time - _startTime;
        }
    }

    public static float RemainingTime
    {
        get
        {
            return (_startTime + Instance.totalTime) - Time.time;
        }
    }

    public delegate void TimeEvent();
    public static event TimeEvent OnTimerEnd;

    private void Awake()
    {
        _startTime = Time.time;
        ClockAnimator.Instance.scale = 3600 / totalTime;
    }

    private void Update()
    {
        if(Time.time >= (_startTime + totalTime) && _running)
        {
            _running = false;
            if(OnTimerEnd != null)
            {
                OnTimerEnd();
            }
        }
    }
}
