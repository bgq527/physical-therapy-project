﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variable_holder : MonoBehaviour
{
    public static Vector3 eyeRotation;
    public static bool calibrated;
    // Start is called before the first frame update
    void Start()
    {
        eyeRotation = new Vector3();
        calibrated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}