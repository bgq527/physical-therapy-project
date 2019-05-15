using System.Collections;
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
    int numTrials;

    TrialData thisTrialData;
    RawData currentRawData;

    // Use this for initialization
    void Start () {

        // GameObject component instantiation
        wallsRenderer = GameObject.Find("Walls").GetComponentsInChildren<Renderer>();
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<TextMesh>();
        cameraTransform = GameObject.Find("ARCamera").GetComponent<Transform>();
        cameraCamera = GameObject.Find("ARCamera").GetComponent<Camera>();


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


        // Debugging variable instantiation
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "start stage";


        // Sets correctness visualization walls to invisible 
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
        
        frameCounter++;
        // 60 fps * .25 seconds = 15 frames

        // The following checks which state the program is in
        switch (state)
        {
            case 0: // Instantiates a new RawData object, determines and sets start time        -> Goes to state 1
                if (currentRawData == null)
                {
                    currentRawData = new RawData();
                    currentRawData.shownArrows = arrows;
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
                    currentRawData.isCorrect = checkCorrect(cameraY, arrows);
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
                if (!((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f)))
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
                    numTrials++;

                    if (numTrials >= 12)
                    {
                        state = 5;
                    }
                    else state = 0;

                    
                    currentRawData.returnedToOrigin = DateTime.UtcNow;
                    thisTrialData.rawUserData.Add(currentRawData);
                    currentRawData = null;

                    for (int i = 0; i < wallsRenderer.Length; i++)
                    {
                        wallsRenderer[i].material.color = new Color(0, 0, 0, 0);

                    }
                    frameCounter = 0;
                    arrowTextMesh.text = arrows;
                    

                    
                }
                break;

            case 5:
                state = 6;
                thisTrialData.packageData();

                string objectToJSON = JsonUtility.ToJson(thisTrialData, true);
                print(objectToJSON);

                string path = "/storage/emulated/0/"; string folderName = "xyz";

                if (!Directory.Exists(path + folderName))
                {
                    Directory.CreateDirectory(path + folderName);
                }

                SaveFile("AndroidText", path, folderName, objectToJSON);
               
                break;
            case 6:
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }


        debugText.text = numTrials.ToString();
    }

 


    void SaveFile(string fileName, string path, string folderName, string jsonFile)
    {
        System.IO.File.WriteAllText(path + folderName + "/" + fileName + ".txt", jsonFile);
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

}
