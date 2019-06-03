using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPress : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartRecording.startButtonPressed = true;
        StartRecording.stopButtonPressed = false;
        print("this is working");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
