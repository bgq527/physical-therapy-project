﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ArrowInstantiation : MonoBehaviour {
    int arrowDirection;
    int frameCounter;
    int state;
    Text arrowTextMesh;
    Transform cameraTransform;
    string arrows;
    Camera cameraCamera;
    string[] arrowText;
    int currentArrow;
    int startFrame;
    Text debugText;
    int numTrials;
    Text leftTarget;
    Text rightTarget;
    GameObject roomScene;
    bool timedSection;

    Statistics stats;
    TrialData thisTrialData;
    RawData currentRawData;

    // Use this for initialization
    void Start () {

        // GameObject component instantiation
        //wallsRenderer = GameObject.Find("Walls").GetComponentsInChildren<Renderer>();
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<Text>();
        //cameraTransform = GameObject.Find("ARCamera").GetComponent<Transform>();
        //cameraCamera = GameObject.Find("ARCamera").GetComponent<Camera>();


        // Statistics and general program variable instantiation
        stats = new Statistics();
        arrowText = new string[4] {"<<<<<", ">>>>>", "<<><<", ">><>>"};
        arrows = arrowText[createArrows()];
        thisTrialData = new TrialData();
        currentRawData = null;

        // Update method variable instantiation
        frameCounter = 0;
        state = 0;
        numTrials = 0;
        timedSection = false;

        // Debugging variable instantiation
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "";

        leftTarget = GameObject.Find("Left").GetComponent<Text>();
        rightTarget = GameObject.Find("Right").GetComponent<Text>();

        roomScene = GameObject.Find("Room");
        

    }

	// Update is called once per frame
	void Update () {
        if (variable_holder.calibrated == true)
        {
            leftTarget.text = "☐";
            rightTarget.text = "☐";
            playArrows();
            //debugText.text = variable_holder.eyeRotation.ToString();
            debugText.text = "";
            roomScene.SetActive(true);

        }
        else
        {
            roomScene.SetActive(false);
            arrowTextMesh.text = "";
            leftTarget.text = "";
            rightTarget.text = "";
        }
    }
    
    void playArrows()
    {
        //debugText.text = state.ToString();
        //float cameraX = cameraTransform.transform.rotation.x;
        //float cameraY = cameraTransform.transform.rotation.y;

        float cameraX = variable_holder.eyeRotation.y;
        float cameraY = variable_holder.eyeRotation.x;


        frameCounter++;
        //arrowTextMesh.text = "";

        if ( timedSection )
        {
            TimeCheck();
        }

        // The following checks which state the program is in
        switch (state)
        {
            case 0: // Instantiates a new RawData object, determines and sets start time        -> Goes to state 1
                if (currentRawData == null)
                {
                    arrowTextMesh.text = arrows;
                    currentRawData = new RawData
                    {
                        shownArrows = arrows,
                        startTime = DateTime.UtcNow
                    };
                    debugText.text = "starttime stage";
                    timedSection = true;
                    
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
                else if (cameraY < -.3f || cameraY > .3f)
                {
                    currentRawData.isCorrect = checkCorrect(cameraY, arrows);
                    currentRawData.hitTarget = DateTime.UtcNow;
                    if (currentRawData.isCorrect)
                    {

                        debugText.text = "correct stage";

                    }
                    else
                    {

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
                if (!(cameraY < -.3f || cameraY > .3f))
                {
                    currentRawData.leftTarget = DateTime.UtcNow;
                    debugText.text = "lefttarg stage";
                    state = 4;

                }
                break;

            case 4: // Determines if
                    // A: The user has returned to the target without hitting the origin        -> Goes to state 3
                    // B: The user has hit the origin                                           -> Goes to state 5 to save the data

                // Scenario A
                if (cameraY < -.3f || cameraY > .3f)
                {
                    currentRawData.leftTarget = DateTime.MinValue;
                    debugText.text = "rettarg stage";
                    state = 3;

                }

                // Scenario B
                else if (cameraX > -.05f && cameraX < .05f && cameraY > -.05f && cameraY < .05f || !currentRawData.completedTrial)
                {
                    timedSection = false;

                    numTrials++;

                    if (numTrials >= 12)
                    {
                        state = 5;
                    }
                    else state = 0;

                    currentRawData.returnedToOrigin = DateTime.UtcNow;
                    thisTrialData.rawUserData.Add(currentRawData);
                    currentRawData = null;


                    frameCounter = 0;
                    arrowTextMesh.text = arrows;

                }
                break;

            case 5: // Saves the data to a JSON file
                state = 6;
                thisTrialData.packageData();

                string objectToJSON = JsonUtility.ToJson(thisTrialData, true);
                print(objectToJSON);

                string path = "/storage/emulated/0/"; string folderName = "xyz";

                if (!Directory.Exists(path + folderName))
                {
                    Directory.CreateDirectory(path + folderName);
                }
                string filename = DateTime.Now.ToFileTimeUtc().ToString();
                SaveFile(filename, path, folderName, objectToJSON);

                break;

            case 6: // Stops the tests
                arrowTextMesh.text = "Done";
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }

        //debugText.text = numTrials.ToString();
    }

    // This method saves a JSON file
    void SaveFile(string fileName, string path, string folderName, string jsonFile)
    {
        System.IO.File.WriteAllText(path + folderName + "/" + fileName + ".JSON", jsonFile);
    }

    // This method checks if the user looked at the correct target
    public bool checkCorrect(float y, string currentArrow)
    {
        bool correct = false;
        if (y > .3f) // checks if the user was looking at the right
        {
            correct = currentArrow[2] == '>';
        }
        else if (y < -.3f) // check if the user was looking at the left
        {
            correct = currentArrow[2] == '<';
        }

        return correct;
    }

    // This method randomly picks a number from 0 to 4 (non-inclusive) to choose between one of four possible arrow configurations
    public int createArrows()
    {
        return (int) UnityEngine.Random.Range(0f, 3.999f);
    }

    public void TimeCheck()
    {
        if (DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond > 250f && (arrowTextMesh.text != "" || arrowTextMesh.text != "+"))
        {
            arrowTextMesh.text = "";
        }
        if (DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond > 1000f)
        {
            currentRawData.completedTrial = false;
            state = 4;
            timedSection = false;
        }
    }

}
