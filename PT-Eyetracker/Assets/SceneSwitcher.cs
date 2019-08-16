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
        timerStopwatch.Restart();
        variable_holder.startButtonPressed = true;
        recordingText.SetActive(true);
        timerText.SetActive(true);
    }

    public void StopRecording()
    {
        variable_holder.stopButtonPressed = true;
        recordingText.SetActive(false);
        timerText.SetActive(false);
        variable_holder.startButtonPressed = false;
        timerStopwatch.Stop();
    }
}