﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class AnalogTime
{
    public int Hour;
    public int Minute;
    public int Second;
}

public class ClockAnimator : MonoBehaviour {
    private DateTime startTime;
    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f,
        secondsToDegrees = 360f / 60f;
    public Transform hours, minutes, seconds;
    public AnalogTime startAnalogTime;
    public float scale = 1f;

    // Use this for initialization
    void Start () {
        startTime = new DateTime(1, 1, 1,
            startAnalogTime.Hour,
            startAnalogTime.Minute,
            startAnalogTime.Second);
	}
	
	// Update is called once per frame
	void Update () {
        DateTime time = startTime.AddSeconds(Time.timeSinceLevelLoad * scale);
        hours.localRotation = Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
        minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
        seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
    }
}