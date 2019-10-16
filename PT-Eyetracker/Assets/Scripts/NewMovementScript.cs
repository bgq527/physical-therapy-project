using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovementScript : MonoBehaviour
{
    public void NewMovements()
    {
        fileHolder.movementFilename = "";
        jsontrainer.loaded = false;
        jsontrainer.replayCount = 0;
        // we can add others such as reseting the timer here as well.
    }
}
