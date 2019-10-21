using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArrowsController : MonoBehaviour
{
    public GameObject room;
    

    void Start()
    {
        room = GameObject.FindGameObjectWithTag("room");
        EnableBackground();
    }

    // Changes current scene to Main Menu
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        //    variable_holder.calibrated = false;

        variable_holder.startButtonPressed = false;
        if (VariableHolder.startedTest) VariableHolder.endedTest = true;
    }

    public void GotoFlanker()
    {
        SceneManager.LoadScene("FlankerScene");
    }

    public void EnableBackground()
    {
        room.SetActive(!room.activeSelf);
    }

    public void StartButtonClicked()
    {
        variable_holder.startButtonPressed = true;
    }

    public void ReloadScene()
    {
        variable_holder.startButtonPressed = false;
    }

    public void LoadCalibration()
    {
        SceneManager.LoadScene("CalibrationScene");
        if (VariableHolder.startedTest) VariableHolder.endedTest = true;
        variable_holder.startButtonPressed = false;
    }


}
