using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArrowsController : MonoBehaviour
{
    // Changes current scene to Main Menu
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
