using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("ArrowsScene");
        XRSettings.enabled = true;   
    }
}
