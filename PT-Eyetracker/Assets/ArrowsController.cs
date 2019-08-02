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
    }

    // Changes current scene to Main Menu
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
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
}
