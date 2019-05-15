using System.Collections;
using System.Collections.Generic;
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
        arrowText = new string[4] {"<<<<<", ">>>>>", "<<><<", ">><>>"};
        arrows = arrowText[createArrows()];
        state = 0;
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        thisTrialData = new TrialData();

        currentRawData = null;
        debugText.text = "start stage";

        for (int i = 0; i < wallsRenderer.Length; i++)
        {
            wallsRenderer[i].material.color = new Color(0, 0, 0, 0);
        }
        arrowTextMesh.text = arrows;

    }

	// Update is called once per frame
	void Update () {
        //debugText.text = state.ToString();
        float cameraX = cameraTransform.transform.rotation.x;
        float cameraY = cameraTransform.transform.rotation.y;
        
        
        //arrowTextMesh.text = Mathf.Round(cameraX * 1000f) / 1000f + ", " + Mathf.Round(cameraY * 1000f) / 1000f;

        // The following checks which state the program is in

        frameCounter++;
        // 60 fps * .25 seconds = 15 frames

        switch (state)
        {
            case 0: // Instantiates a new RawData object, determines and sets start time        -> Goes to state 1
                if (currentRawData == null)
                {
                    currentRawData = new RawData();
                    currentRawData.startTime = DateTime.UtcNow;
                    debugText.text = "starttime stage";
                    state = 1;

                }
                break;

            case 1: // Determines if the user has left the origin                               -> Goes to state 2
                if (!(cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f))
                {
                    currentRawData.leftOrigin = DateTime.UtcNow;
                    debugText.text = "leftorig stage";
                    state = 2;

                }
                break;

            case 2: // Determines if
                    // A: The user has returned to the origin without hitting a target          -> Goes back to state 1
                    // B: The user has hit a target                                             -> Goes to state 3

                // Scenario A
                if (cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f)
                {
                    currentRawData.leftOrigin = DateTime.MinValue;
                    debugText.text = "retorig w/o targethit stage";
                    state = 1;

                }

                // Scenario B
                else if ((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f))
                {
                    currentRawData.isCorrect = checkCorrect(cameraY);
                    currentRawData.hitTarget = DateTime.UtcNow;
                    if (currentRawData.isCorrect)
                    {
                        for (int i = 0; i < wallsRenderer.Length; i++)
                        {
                            wallsRenderer[i].material.color = new Color(0f, 1f, 0f, .4f);

                        }
                        debugText.text = "correct stage";

                    }
                    else
                    {
                        for (int i = 0; i < wallsRenderer.Length; i++)
                        {
                            wallsRenderer[i].material.color = new Color(1f, 0f, 0, .4f);
                        }
                        debugText.text = "incorr stage";

                    }

                    arrowTextMesh.text = "+";
                    arrows = arrowText[createArrows()];
                    currentRawData.completedTrial = true;
                    state = 3;

                }
                Console.WriteLine("Case 2");
                break;

            case 3: // Determines if the user has left the target threshold                     -> Goes to state 4
                if (!((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f)) && currentRawData.leftTarget == DateTime.MinValue)
                {
                    currentRawData.leftTarget = DateTime.UtcNow;
                    debugText.text = "lefttarg stage";
                    state = 4;

                }
                break;

            case 4: // Determines if
                    // A: The user has returned to the target without hitting the origin        -> Goes to state 3
                    // B: The user has hit the origin                                           -> Goes to state 0

                // Scenario A
                if ((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f))
                {
                    currentRawData.leftTarget = DateTime.MinValue;
                    debugText.text = "rettarg stage";
                    state = 3;

                }

                // Scenario B
                else if (cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f)
                {
                    debugText.text = "retorig stage";
                    currentRawData.returnedToOrigin = DateTime.UtcNow;
                    thisTrialData.rawUserData.Add(currentRawData);
                    currentRawData = null;

                    for (int i = 0; i < wallsRenderer.Length; i++)
                    {
                        wallsRenderer[i].material.color = new Color(0, 0, 0, 0);

                    }
                    frameCounter = 0;
                    arrowTextMesh.text = arrows;
                    state = 0;

                }
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }

    }

    // This method checks if the user looked at the correct target
    public bool checkCorrect(float y)
    {
        bool correct = false;
        if (y > .2f) // checks if the user was looking at the right
        {
            correct = currentArrow == 1 || currentArrow == 2;
        }
        else if (y < -.2f) // check if the user was looking at the left
        {
            correct = currentArrow == 0 || currentArrow == 3;
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
