using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Arrows : MonoBehaviour
{
    int state;
    Text arrowTextMesh;
    string currentArrows;
    string[] possibleArrows;
    int numTrials;
    Text leftTarget;
    Text rightTarget;
//    GameObject roomScene;
    bool isSceneSetup;
    bool timeOne;
    float originThreshold;

    Statistics stats;
    TrialData thisTrialData;
    RawData currentRawData;

    Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        // GameObject component instantiation
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<Text>();
        leftTarget = GameObject.Find("Left").GetComponent<Text>();
        rightTarget = GameObject.Find("Right").GetComponent<Text>();
//      roomScene = GameObject.Find("Room");

        // Statistics and general program variable instantiation
        stats = new Statistics();
        possibleArrows = new string[4] { "<<<<<", ">>>>>", "<<><<", ">><>>" };
    //  currentArrows = possibleArrows[CreateArrows()];
        thisTrialData = new TrialData();
        currentRawData = null;

        // Update method variable instantiation
        state = 0;
        numTrials = 0;
        isSceneSetup = false;
        originThreshold = .1f;

        // Debugging variable instantiation
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "";

        // hide objects before eye tracker is calibrated
//      roomScene.SetActive(false);
        arrowTextMesh.enabled = false;
        leftTarget.text = "";
        rightTarget.text = "";

    }

    // Update is called once per frame
    void Update()
    {

        //debugText.text = state.ToString();

        // Check if the scene has already been setup
        if (!isSceneSetup)
        {
            // Once the eye tracker has been calibrated setup the scene
           if (variable_holder.calibrated == true)
            {
                SetupScene();
            }
        }

        // If the scene has been setup and it is not the final scene, PlayArrows()
        else if (state != 6 && isSceneSetup)
        {
            PlayArrows();
        }
    }

    // This method is called once every frame once the eyetracker has been calibrated.
    // It gives the Eriksen Flanker task to the user and then saves the results
    void PlayArrows()
    {
        float cameraX = variable_holder.eyeRotation.y;
        float cameraY = variable_holder.eyeRotation.x;
        DateTime currentTime = DateTime.UtcNow;

        /* For Debugging
         * 
         * debugText.text = cameraX+","+cameraY+"state:"+state;
         * if (state != 1)
         * debugText.text = ""+(currentTime - currentRawData.startTime).Milliseconds;
        */

        // Check if the the state is not 1 as TimeTracker() would not work correctly in state 1
        // State 1 resets the variables so TimeTracker() would use old data if it was called during state 1;
        if (state != 1) TimeTracker();

        // Since this method is run every frame we use states to keep track of where the user is at in the test
        // State 1: is the time when it starts, it can only last 1 frame as everytime it is called it sets state = 2
        // State 2: starts when the user is looking in the origin threshold and ends when they leave the threshold
        // State 3: starts when the user leaves the origin and ends when the user hits a target
        // State 4: starts when the user has hit the target and ends when the user has left the target
        // State 5: starts when the user has reentered the origin threshold, it sets the state to either 1 or 6 depending on the number of trials
        // State 6: the last state, currently not utilised, but will be used when support for multiple exercises is added

        switch (state)
        {

            // State 1: reset variables, goes to state 2 
            case 1:
                timeOne = false;
                
                // Generate a new set of arrows
                currentArrows = possibleArrows[CreateArrows()];

                // Create a new RawData object and record the shown arrows and startTime
                currentRawData = new RawData
                {
                    shownArrows = currentArrows,
                    startTime = DateTime.UtcNow
                };

                // Set the text to the new set of arrows
                arrowTextMesh.text = currentArrows;
                state = 2;

                break;

            // State 2: check if user leaves origin, goes to state 3
            case 2:
                // Check if the user has left the center
                if (!CenterCheck(cameraX, cameraY))
                {
                    currentRawData.leftOrigin = DateTime.UtcNow;
                    state = 3;
                }

                break;

            // State 3: check if the user hits a target, goes to state 4
            case 3:
                // check if they user has hit a target
                if (cameraY < -.3f || cameraY > .3f)
                {
                    // Check if the user selected the correct target
                    currentRawData.isCorrect = CheckCorrect(cameraY, currentArrows);
                    currentRawData.hitTarget = DateTime.UtcNow;
                    currentRawData.completedTrial = true;

                    // Set the text to a "+" to indicate to the player to look back at it
                    arrowTextMesh.text = "+";

                    state = 4;
                }
                break;

            // State 4: check if the user has left the target, goes to state 5
            case 4:
                if (!(cameraY < -.3f || cameraY > .3f))
                {
                    currentRawData.leftTarget = DateTime.UtcNow;
                    state = 5;
                }
                break;

            // State 5: check is the user has reenter the origin, goes to state 1 or 6 depeding on numTrials
            case 5:
                if (CenterCheck(cameraX, cameraY))
                {
                    numTrials++;

                    // check all 12 trials have occured
                    // if they have go to state 6 to save the data
                    // if less than 12 have occured go to state 1
                    state = numTrials < 12 ? 1 : 6;

                    currentRawData.returnedToOrigin = DateTime.UtcNow;
                    thisTrialData.rawUserData.Add(currentRawData);
                    currentRawData = null;

                    if (numTrials >= 12) SaveData();
                }
                break;

            // State 6
            case 6:
                // Empty for now but we can use when one person has to perform multiple exercises/tests
                break;

            default:
                Debug.Log("Default state");
                break;
        }
    }

    // This method randomly picks a number from 0 to 4 (non-inclusive) to choose between one of four possible arrow configurations
    int CreateArrows()
    {
        return (int)UnityEngine.Random.Range(0f, 3.9999f);
    }

    // This method sets all the necessary variable for the scene 
    void SetupScene()
    {
        leftTarget.text = "☐";
        rightTarget.text = "☐";
        debugText.text = "";
        state = 1;
        arrowTextMesh.enabled = true;
//      roomScene.SetActive(true);
        isSceneSetup = true;
    }

    // A method to make sure the tests stay ontime (1000 ms per set of arrows) and that the arrows are only shown for 250 ms
    void TimeTracker()
    {
        // Checks if 250 ms have passed since the initial showing of the arrows
        // makes the arrows disappear if time has passed
        if (DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond > 250f && !timeOne)  {
            arrowTextMesh.text = "";
            timeOne = true;
        }
        
        // checks if 1 second has passed since the initial showing of arrows
        // moves on to the next set of arrows if the user has not completed in 1 second
        else if ((DateTime.UtcNow - currentRawData.startTime).Seconds > 1f) {
            currentRawData.completedTrial = false;
            thisTrialData.rawUserData.Add(currentRawData);
            currentRawData = null;
            numTrials++;
            if (numTrials < 12)
            {
                state = 1;
            }
            else
            {
                state = 6;
                SaveData();
            }
                
        }
    }

    // A method to check whether the the user is looking inside the center threshold
    bool CenterCheck(float x, float y)
    {
        return (Math.Sqrt((x - 0) * (x - 0) + (y - 0) * (y - 0)) <= originThreshold);
    }

    // A method to check if the user was looking at the correct target
    bool CheckCorrect(float y, string currentArrow)
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

    // This method saves the trial data to a JSON file
    void SaveData()
    {
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

        System.IO.File.WriteAllText(path + folderName + "/" + filename + ".JSON", objectToJSON);

        arrowTextMesh.text = "done";
    }
}
