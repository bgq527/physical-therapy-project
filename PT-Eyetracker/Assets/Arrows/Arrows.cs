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

    public bool roomActive;
    bool isSceneSetup;
    bool timeOne;
    public static float originThreshold = .1f;
    bool countDownFinished;
    bool countDownStarted;
    DateTime countDownStartTime;

    Statistics stats;
    TrialData thisTrialData;
    RawData currentRawData;

    Text debugText;

    public GameObject RightConfText;
    public GameObject LeftConfText;
    public GameObject CurrentTimingText;
    public GameObject NextSetText;
    public GameObject CurrentStageText;
    public GameObject PreviousCompletedText;
    public GameObject PreviousCorrectText;
    public GameObject PreviousLeaveOriginText;
    public GameObject PreviousHitTargetText;
    public GameObject PreviousLeftTargetText;

    public GameObject LineRenderObject;

    public bool showThresholds;
    private int frame;

    public static int thresholdPointsToDraw = 30;
    public static int hitmarkerPointsToDraw = 4;

    public static float arrowTiming = 250f;
    public static float setTiming = 1f;

    public static float targetX = .3f;
    public static float targetY = 0f;
    public static float scale = 1.0f;
    public GameObject LeftTarget;
    public GameObject RightTarget;

    public Vector3[] lTarget;
    public Vector3[] rTarget;

    private bool drawtargets = false;


    // Start is called before the first frame update
    void Start()
    {
        // GameObject component instantiation
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<Text>();
        leftTarget = GameObject.Find("Left").GetComponent<Text>();
        rightTarget = GameObject.Find("Right").GetComponent<Text>();
   //   roomScene = GameObject.Find("Room");

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
    //    originThreshold = .1f;
        countDownFinished = false;
        roomActive = true;

        // Debugging variable instantiation
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "";

        // hide objects before eye tracker is calibrated
        //      roomScene.SetActive(false);

        arrowTextMesh.color = Color.gray;
        leftTarget.color = Color.gray;
        rightTarget.color = Color.gray;

        arrowTextMesh.enabled = false;
        leftTarget.text = "";
        rightTarget.text = "";

       // LineRenderObject = GameObject.Find("LineRenderObject");

        showThresholds = true;
        frame = 0;

        lTarget = new Vector3[5];
        rTarget = new Vector3[5];

        ChangeTargets(0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (showThresholds)
        {
           ShowOrigin();
           DrawEyeHitMarker(variable_holder.eyeRotation.x, variable_holder.eyeRotation.y);
        }
        if (drawtargets) DrawTargets();

        //originMarker.transform
        //debugText.text = state.ToString();
        UpdateStats();
        // Check if the scene has already been setup
        if (!isSceneSetup)
        {
            
            // Once the eye tracker has been calibrated setup the scene
           if (variable_holder.calibrated == true && variable_holder.startButtonPressed)
            {
                CountDown();
                if (countDownFinished) SetupScene();
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

        if (state > 1 && state < 4) currentRawData.gazeCoords = (new Vector2(cameraX, cameraY));
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
               // leftTarget.text = "☐";
               // rightTarget.text = "☐";
                state = 2;
                drawtargets = true;

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
                if (CheckHitTarget(cameraX, cameraY))
                {
                    // Check if the user selected the correct target
                    currentRawData.isCorrect = CheckCorrect(cameraY, currentArrows);
                    currentRawData.hitTarget = DateTime.UtcNow;
                    

                    // Set the text to a "+" to indicate to the player to look back at it
                    arrowTextMesh.text = "+";
                    leftTarget.text = "";
                    rightTarget.text = "";
                    state = 4;

                    drawtargets = false;
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
                    currentRawData.completedTrial = true;
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

    // Counts down from 5 seconds
    void CountDown()
    {
        if (!countDownStarted)
        {
            countDownStartTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            arrowTextMesh.enabled = true;
            countDownStarted = true;
        }

        arrowTextMesh.text =  countDownStartTime.Second - DateTime.UtcNow.Second  + "";

        if (DateTime.UtcNow.Second - countDownStartTime.Second > 0f)
        {
            countDownFinished = true;
        }

    }

    // Updates the stats box
    void UpdateStats()
    {
        //Text[] text = RightConfText.GetComponents<Text>();
        //text[0].text = Time.time.ToString();
        
        Text rightText = RightConfText.GetComponent<Text>();
        Text stageText = CurrentStageText.GetComponent<Text>();
        Text timingText = CurrentTimingText.GetComponent<Text>();
        Text nextText = NextSetText.GetComponent<Text>();
        Text leaveOrigText = PreviousLeaveOriginText.GetComponent<Text>();
        Text correctText = PreviousCorrectText.GetComponent<Text>();
        Text completedText = PreviousCompletedText.GetComponent<Text>();
        Text hitTargText = PreviousHitTargetText.GetComponent<Text>();
        Text leaveTargText = PreviousLeftTargetText.GetComponent<Text>();

        //LeftConfText

        rightText.text = variable_holder.conf+"";


        nextText.text = currentArrows;

        
        if (state > 0)
        {
            //if (state != 1)
            //{
            //    completedText.text = currentRawData.returnedToOrigin.Millisecond - currentRawData.startTime.Millisecond + "";
            //    correctText.text = currentRawData.isCorrect + "";
            //    leaveOrigText.text = currentRawData.leftOrigin.Millisecond - currentRawData.startTime.Millisecond + "";
            //    hitTargText.text = currentRawData.hitTarget.Millisecond - currentRawData.startTime.Millisecond + "";
            //    leaveTargText.text = currentRawData.leftTarget.Millisecond - currentRawData.startTime.Millisecond + "";
            //}



            //CurrentStageText
            switch (state)
            {
                case 1:
                    stageText.text = "reset state";
                    break;
                case 2:
                    stageText.text = "in origin";
             //       timingText.text = DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond + "";
                    break;
                case 3:
                    stageText.text = "left origin";
            //        timingText.text = DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond + "";
                    break;
                case 4:
                    stageText.text = "hit target";
            //        timingText.text = DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond + "";
                    break;
                case 5:
                    stageText.text = "left target";
            //        timingText.text = DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond + "";
                    break;
                case 6:
                    stageText.text = "test end";
            //        timingText.text = DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond + "";
                    break;

            }
        }
        else if (variable_holder.calibrated) stageText.text = "waiting for start";
        else stageText.text = "calibration";
        
        
        //PreviousCompletedText
        //PreviousCorrectText
        //PreviousLeaveOriginText
        //PreviousHitTargetText
        //PreviousLeftTargetText

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
        if (DateTime.UtcNow.Millisecond - currentRawData.startTime.Millisecond > arrowTiming && !timeOne)  {
            arrowTextMesh.text = "";
            timeOne = true;
        }
        
        // checks if 1 second has passed since the initial showing of arrows
        // moves on to the next set of arrows if the user has not completed in 1 second
        else if (((DateTime.UtcNow - currentRawData.startTime).Milliseconds/1000 + (DateTime.UtcNow - currentRawData.startTime).Seconds) > setTiming) {
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

    // Changes color of text when background changes
    public void RoomChanged()
    {
        roomActive = !roomActive;
        if (roomActive)
        {
            arrowTextMesh.color = Color.black;
            leftTarget.color = Color.black;
            rightTarget.color = Color.black;
        }
        else
        {
            arrowTextMesh.color = Color.white;
            leftTarget.color = Color.white;
            rightTarget.color = Color.white;

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
        string csv = thisTrialData.packageData();
        string objectToJSON = JsonUtility.ToJson(thisTrialData, true);
        print(objectToJSON);

        string path = "/storage/emulated/0/"; string folderName = "xyz";

        if (!Directory.Exists(path + folderName))
        {
            Directory.CreateDirectory(path + folderName);
        }
        string filename = DateTime.Now.ToFileTimeUtc().ToString();

        System.IO.File.WriteAllText(path + folderName + "/" + filename + ".csv", csv);
        System.IO.File.WriteAllText(path + folderName + "/" + filename + ".JSON", objectToJSON);

        arrowTextMesh.text = "done";

        CallSaveData();

    }

    public void CallSaveData()
    {
        StartCoroutine(SaveDataSQL());
    }


    IEnumerator SaveDataSQL()
    {
        WWWForm form = new WWWForm();
        form.AddField("avgtime", variable_holder.dataholder[0] + "");
        form.AddField("contime", variable_holder.dataholder[1] + "");
        form.AddField("incontime", variable_holder.dataholder[2] + "");
        form.AddField("conflicteffect", variable_holder.dataholder[3] + "");
        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("Data saved successfully.");
        }
        else
        {
            Debug.Log("Save Data failed. Error: " + www.text);
        }

    }

    private bool CheckHitTarget(float x, float y)
    {
        if (x < rTarget[0].x && x > rTarget[3].x && y < rTarget[0].y && y > rTarget[3].y) return true;
        else if (x < lTarget[0].x && x > lTarget[3].x && y < lTarget[0].y && y > lTarget[3].y) return true;
        else return false;
    }

    private void ShowOrigin()
    {
        double xpoint;
        double ypoint;
        Vector3[] points = new Vector3[60];
        for (int i = 0; i < 360; i += 6)
        {
            xpoint = originThreshold * Math.Cos(i) + LineRenderObject.transform.position.x;
            ypoint = originThreshold * Math.Sin(i) + LineRenderObject.transform.position.y;
            points[i/6] = new Vector3(Convert.ToSingle(xpoint), Convert.ToSingle(ypoint), LineRenderObject.transform.position.z);
        }
        LineRenderer originMarker = LineRenderObject.GetComponent<LineRenderer>();
        originMarker.material = new Material(Shader.Find("Sprites/Default"));
        originMarker.widthMultiplier = 0.005f;
        originMarker.positionCount =thresholdPointsToDraw;
        originMarker.SetPositions(points);
    }

    private void DrawEyeHitMarker(float x, float y)
    {
        GameObject GO = GameObject.Find("EyeHitMarker");
        LineRenderer hitmarker = GO.GetComponent<LineRenderer>();
        hitmarker.material = new Material(Shader.Find("Sprites/Default"));
       
        hitmarker.widthMultiplier = .01f;

        if (variable_holder.conf > .75f) hitmarker.material.color = Color.green;
        else hitmarker.material.color = Color.red;

        double xpoint;
        double ypoint;
        Vector3[] points = new Vector3[15];
        for (int i = 0; i < 360; i+=24)
        {
            xpoint = .005f * Math.Cos(i) + (GO.transform.position.x+x);
            ypoint = .005f * Math.Sin(i) + (GO.transform.position.y+y);
            points[i/24] = new Vector3(Convert.ToSingle(xpoint), Convert.ToSingle(ypoint), GO.transform.position.z);
        }

        hitmarker.positionCount = hitmarkerPointsToDraw;
        hitmarker.SetPositions(points);
    }

    // Turn on/off showing the thresholds
    public void changeThresholdChoice()
    {
        showThresholds = !showThresholds;
    }

    public void DrawTargets()
    {
        LineRenderer leftLR = LeftTarget.GetComponent<LineRenderer>();
        LineRenderer rightLR = RightTarget.GetComponent<LineRenderer>();

        rightLR.positionCount = 5;
        rightLR.material = new Material(Shader.Find("Sprites/Default"));
        rightLR.material.color = Color.red;
        rightLR.widthMultiplier = .005f;

        leftLR.positionCount = 5;
        leftLR.material = new Material(Shader.Find("Sprites/Default"));
        leftLR.material.color = Color.red;
        leftLR.widthMultiplier = .005f;

        rightLR.SetPositions(rTarget);
        leftLR.SetPositions(lTarget);

    }

    public void ChangeTargets(float xvalue, float scale)
    {

        float size = 20 * scale / 2;
        float x = .3f + xvalue;

        Vector3[] points = new Vector3[5];
        
        // Right
        points[0] = new Vector3(
            RightTarget.transform.position.x + .3f,
            0.005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[1] = new Vector3(
            RightTarget.transform.position.x + .3f,
            .005f * -size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[3] = new Vector3(
            2 * size * .005f + RightTarget.transform.position.x + .3f,
            .005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[2] = new Vector3(
            2 * size * .005f + RightTarget.transform.position.x + .3f,
            .005f * -size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[4] = new Vector3(
            RightTarget.transform.position.x + .3f,
            0.005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );

        rTarget = points;

        // Left
        points[0] = new Vector3(
            RightTarget.transform.position.x - .3f,
            0.005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[1] = new Vector3(
            RightTarget.transform.position.x - .3f,
            .005f * -size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[3] = new Vector3(
            -2 * size * .005f + RightTarget.transform.position.x - .3f,
            .005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[2] = new Vector3(
            -2 * size * .005f + RightTarget.transform.position.x - .3f,
            .005f * -size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );
        points[4] = new Vector3(
            RightTarget.transform.position.x - .3f,
            0.005f * size + RightTarget.transform.position.y,
            RightTarget.transform.position.z
            );

        lTarget = points;
    }

}
