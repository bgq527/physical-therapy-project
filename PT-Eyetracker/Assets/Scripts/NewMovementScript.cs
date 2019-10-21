using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMovementScript : MonoBehaviour
{
    public void NewMovements()
    {
        fileHolder.movementFilename = "";
        jsontrainer.loaded = false;
        jsontrainer.replayCount = 0;
        // we can add others such as reseting the timer here as well.
    }

    public void ChangeMarkers()
    {
        //var GO = GameObject.Find("MarkerToggle");
        //bool isEnabled = GO.GetComponent<Toggle>().isOn;

        //if (isEnabled)
        //{
        //    threshold_movment_1_model.currentMovement = 4;
        //    var text = GameObject.Find("MarkerInfoText").GetComponent<Text>();
        //    text.text = "Displaying markers for: Markers disabled";
        //}
        threshold_movment_1_model.ChangeMovement();

    }
}
