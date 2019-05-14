﻿using System.Collections;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArrowInstantiation : MonoBehaviour {
    int arrowDirection;
    int frameCounter;
    int state;
    TextMesh arrowTextMesh;
    Transform cameraTransform;
    string arrows;
    Camera cameraCamera;
    string[] arrowText;
    int currentArrow;
    int startFrame;
    Renderer[] wallsRenderer;
    Statistics stats;
    Text debugText;

    TrialData thisTrialData;
    RawData currentRawData;

    // Use this for initialization
    void Start () {
        wallsRenderer = GameObject.Find("Walls").GetComponentsInChildren<Renderer>();
        frameCounter = 0;
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<TextMesh>();
        cameraTransform = GameObject.Find("ARCamera").GetComponent<Transform>();
        cameraCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        stats = new Statistics();
        arrowText = new string[4] {"<<<<<", ">>>>>", "<<><<", ">><>>" };
        arrows = arrowText[createArrows()];
        //state = 1;
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        currentRawData = null;
        thisTrialData = new TrialData();

        currentRawData.returnedToOrigin = DateTime.UtcNow;
        thisTrialData.rawUserData.Add(currentRawData);
        currentRawData = null;
        debugText.text = "retorig stage";

        for (int i = 0; i < wallsRenderer.Length; i++)
        {
            wallsRenderer[i].material.color = new Color(0, 0, 0, 0);
        }
        frameCounter = 0;
        arrowTextMesh.text = arrows;
        state = 0;

    }

	// Update is called once per frame
	void Update () {
        //arrowTextMesh.text = cameraTransform.transform.rotation.x + " | " + cameraTransform.transform.rotation.y;
        float cameraX = cameraTransform.transform.rotation.x;
        float cameraY = cameraTransform.transform.rotation.y;
        
        
        //arrowTextMesh.text = Mathf.Round(cameraX * 1000f) / 1000f + ", " + Mathf.Round(cameraY * 1000f) / 1000f;

        // The following checks which state the program is in
        // state 0 is the time when the test is currently active
        // it starts when the user looks at the center and ends when the user looks at a target

        // state 1 is the time in between tests when the user is recentering their head
        // it begins when the user looks at the target and ends when the user looks at the "Look here" target

        frameCounter++;
        // 60 fps * .25 seconds = 15 frames

        if (state == 0)
        {
            if (currentRawData == null)
            {
                currentRawData = new RawData();
                currentRawData.startTime = DateTime.UtcNow;
                debugText.text = "starttime stage";
            }
            // Makes the command disappear after ~250 ms
            if (frameCounter > 45)
            {
                //arrowTextMesh.text = "+";
            }

           
            // Checks if person left the origin
            if (!(cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f) && currentRawData.leftOrigin == DateTime.MinValue)
            {
                currentRawData.leftOrigin = DateTime.UtcNow;
                debugText.text = "leftorig stage";
            }

            // Checks if person returned to the origin before hitting a target
            if ((cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f) && currentRawData.leftOrigin != DateTime.MinValue)
            {
                currentRawData.leftOrigin = DateTime.MinValue;
                debugText.text = "retorig w/o targethit stage";
            }

            // Checks if the user hit either target
            if (((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f)) && currentRawData.hitTarget == DateTime.MinValue)
            {
                currentRawData.hitTarget = DateTime.UtcNow;
                if (checkCorrect(cameraY))
                {
                     for (int i = 0; i < wallsRenderer.Length; i++){
                       wallsRenderer[i].material.color = new Color(1f, 0f, 0, .4f);
                     }
                    currentRawData.isCorrect = true;
                    debugText.text = "correct stage";
                }
                else
                {
                    for (int i = 0; i < wallsRenderer.Length; i++){
                      wallsRenderer[i].material.color = new Color(0f, 1f, 0, .4f);
                    }
                    currentRawData.isCorrect = false;
                    debugText.text = "incorr stage";
                }

                state = 1;
                arrowTextMesh.text = "+";
                currentArrow = createArrows();
                arrows = arrowText[currentArrow];
                currentRawData.completedTrial = true;
            }
            // if 1 second has passed start new command to keep pace
            else if (frameCounter > 180)
            {
                state = 1;
                arrowTextMesh.text = "+";
                currentArrow = createArrows();
                arrows = arrowText[currentArrow];
                currentRawData.completedTrial = false;
                debugText.text = "didnotcomp stage";
            }
        }

        if (state == 1)
        {
            // Checks if person left target
            if (!((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f)) && currentRawData.leftTarget == DateTime.MinValue)
            {
                currentRawData.leftTarget = DateTime.UtcNow;

                state = 1;
                arrowTextMesh.text = "+";
                currentArrow = createArrows();
                arrows = arrowText[currentArrow];
                currentRawData.completedTrial = true;

                debugText.text = "lefttarg stage";
            }

            // Checks if person returned to target without returning to origin
            if (((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f)) && currentRawData.leftTarget != DateTime.MinValue)
            {
                currentRawData.hitTarget = DateTime.MinValue;
                debugText.text = "rettarg stage";

            }

            // Checks if returned to origin
            if ((cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f) && currentRawData.returnedToOrigin == DateTime.MinValue)
            {
                currentRawData.returnedToOrigin = DateTime.UtcNow;
                thisTrialData.rawUserData.Add(currentRawData);
                currentRawData = null;
                debugText.text = "retorig stage";

                for (int i = 0; i < wallsRenderer.Length; i++){
                  wallsRenderer[i].material.color = new Color(0, 0, 0, 0);
                }
                frameCounter = 0;
                arrowTextMesh.text = arrows;
                state = 0;
                
            }

        }
    }

    // This method checks if the user looked at the correct target
    public bool checkCorrect(float y)
    {
        bool correct;
        if (y > .3f) // checks if the user was looking at the right
        {
            correct = (currentArrow==0 || currentArrow==3);
        }
        else // check if the user was looking at the left
        {
            correct = (currentArrow == 1 || currentArrow == 2);
        }

        return correct;
    } // checkCorrect(float)

    // This method creates a String of five arrows pointing in random directions
    public int createArrows()
    {
        return (int) UnityEngine.Random.Range(0f, 3.99f);
    } // createArrows()

    public int correctArrow()
    {
        return currentArrow;
    }
}
