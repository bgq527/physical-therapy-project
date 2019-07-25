using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Method to close application
   public void ExitGame()
    {
        Application.Quit();
    }
    
    // Changes the current scene to the Flanker Task
    public void GotoFlankerScene()
    {
        SceneManager.LoadScene("FlankerScene");
    }

    // Changes the current scene to the PlayMovements scene
    public void GotoPlayMovementsScene()
    {
        SceneManager.LoadScene("PlayMovementsScene");
    }

    // Changes the current scene to SaveMovementsScene 
    public void GotoSaveMovementsScene()
    {
        SceneManager.LoadScene("SaveMovementsScene"); 
    }

    // Changes the current scene to EditParticipant scene
    public void GotoEditParticipantScene()
    {
        SceneManager.LoadScene("EditParticipantScene");
    }

    // Changes the current scene to the settings scene
    public void GotoSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void GotoRGBTest()
    {
        SceneManager.LoadScene("RGBTest");
    }
}

