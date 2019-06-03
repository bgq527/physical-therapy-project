using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRecording : MonoBehaviour
{
    public static bool startButtonPressed;
    public static bool stopButtonPressed;
    public static string saveFileName;

    // Start is called before the first frame update
    void Start()
    {
        startButtonPressed = false;
        stopButtonPressed = false;
        saveFileName = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
