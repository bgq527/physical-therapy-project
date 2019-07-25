using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System;
using System.Text;

public class RGBController : MonoBehaviour
{
    public static bool testStarted;
    PupilLabs.GazeData gazeData;
    Text confidenceText;

    void Start()
    {
        testStarted = false;
        
        
    }

    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void StartTest()
    {
        GameObject.Find("TestController").GetComponent<RGBScript>().enabled = true;
        VariableHolder.startedTest = true;
        //GameObject.Find("StopButton").SetActive(true);
        // GameObject.Find("RemoteController").GetComponent<PupilRemoteController>().enabled = true;
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("RGBTest");
    }

    public void StopTest()
    {
        VariableHolder.endedTest = true;
    }

    
}





