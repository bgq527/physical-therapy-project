using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public static bool startButtonPressed;
    public static bool stopButtonPressed;


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
    }

    public void StopRecording()
    {
        //startButtonPressed = false;
        //stopButtonPressed = true;
        //Debug.Log("StopRecordingPressed");

        variable_holder.stopButtonPressed = true;
    }
}