﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variable_holder : MonoBehaviour
{
    public static Vector3 eyeRotation;
    public static bool calibrated;
    public static float yvalue;
    public static float minPos;
    public static bool startButtonPressed = false;
    public static bool stopButtonPressed = false;

    public static float[] dataholder = new float[4];

    public static float conf;

    public float conTime;
    public float inconTime;
    public float avgTime;
    public float conflictEffect;

    // Start is called before the first frame update
    void Start()
    {
        eyeRotation = new Vector3();
        calibrated = false;
        yvalue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
