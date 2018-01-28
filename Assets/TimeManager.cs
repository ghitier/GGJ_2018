using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager> {

    public float totalTime = 300f;

    private static float _startTime;

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

    private void Awake()
    {
        _startTime = Time.time;
    }
}
