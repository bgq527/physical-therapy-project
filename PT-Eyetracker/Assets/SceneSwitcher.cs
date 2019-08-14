using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;

public class SceneSwitcher : MonoBehaviour
{
    public static bool startButtonPressed;
    public static bool stopButtonPressed;
    GameObject recordingText;
    GameObject timerText;
    Stopwatch timerStopwatch;

    void Start()
    {
        recordingText = GameObject.Find("RecordingText");
        recordingText.SetActive(false);
        timerText = GameObject.Find("TimerText");
        timerText.SetActive(false);
        timerStopwatch = new Stopwatch();

    }

    void Update()
    {
        if (variable_holder.startButtonPressed && ! variable_holder.stopButtonPressed && ! timerStopwatch.IsRunning)
        {
            timerStopwatch.Reset();
            timerStopwatch.Start();
        }
        else if (variable_holder.stopButtonPressed && !timerStopwatch.IsRunning)
        {
            timerStopwatch.Stop();
        }
        // Text rightText = RightConfText.GetComponent<Text>();
        //timerTe
        Text timerTextMesh = timerText.GetComponent<Text>();
        timerTextMesh.text = timerStopwatch.Elapsed.Minutes + ":" + timerStopwatch.Elapsed.Seconds + ":" + timerStopwatch.Elapsed.Milliseconds /* + timerStopwatch.ElapsedMilliseconds*/;
    }

    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
        XRSettings.enabled = true;
    }

    public void ChangeFileName(InputField text)
    {
        fileHolder.saveFilename = text.text;
        print(fileHolder.saveFilename);
    }

    public void PrintConfirmation()
    {
        print(fileHolder.saveFilename);
    }

    public void StartRecording()
    {
        //stopButtonPressed = false;
        //startButtonPressed = true;
        //Debug.Log("StartRecordingPressed");

        //Realtime_Player_Save.saveJSON = true;

        variable_holder.startButtonPressed = true;
        recordingText.SetActive(true);
        timerText.SetActive(true);
    }

    public void StopRecording()
    {
        //startButtonPressed = false;
        //stopButtonPressed = true;
        //Debug.Log("StopRecordingPressed");

        variable_holder.stopButtonPressed = true;
        recordingText.SetActive(false);
        timerText.SetActive(false);
    }
}