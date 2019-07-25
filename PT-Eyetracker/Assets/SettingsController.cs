using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    
    // Changes the current scene to the Main Menu
    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}
