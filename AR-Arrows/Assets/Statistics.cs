using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;

[System.Serializable]
class Statistics : MonoBehaviour
{
    string userName;
    DateTime trialDate;
    List<TrialData> userTrials;


}

[System.Serializable]
class TrialData
{
    public float conTime;
    public float inconTime;

    // max time that they took to hit the target sakadTime
    public float testTime;

    // average of response time 
    public float avgTime;

    // ( numTrials * average response ) / correct answers
    public float efficiency;
    public int correct;


   public List<RawData> rawUserData = new List<RawData>();

    public void packageData ()
    {

        // time to return to target
        TimeSpan responseTime = new TimeSpan();

        // time to hit taget
        TimeSpan sakadTime = new TimeSpan();


        testTime = 0;
        int conCount = 0;
        int inconCount = 0;

        for (int i = 0; i < rawUserData.Count; i++)
        {
            // Response time calculated as the time that it took to go from the target back to the origin
            responseTime = rawUserData[i].leftTarget - rawUserData[i].returnedToOrigin;

            // Sakad time calculated as the time that it took to go from the origin to the target
            sakadTime = rawUserData[i].leftOrigin - rawUserData[i].hitTarget;

            if (rawUserData[i].shownArrows == "<<<<<" || rawUserData[i].shownArrows == ">>>>>" )
            {
                conTime += sakadTime.Milliseconds;
                conCount++;
            }
            else
            {
                inconTime += sakadTime.Milliseconds;
                inconCount++;
            }

            if (rawUserData[i].isCorrect == true)
            {
                correct++;
            }

            if (responseTime.Milliseconds > testTime)
            {
                testTime = responseTime.Milliseconds;
            }

            avgTime += responseTime.Milliseconds;
        }

        // Calculates avgTime
        avgTime = avgTime / rawUserData.Count;

        // Calculates the conTime
        if (conCount == 0) conTime = 0;
        else conTime = conTime / conCount;

        // Calculates the inconTime
        if (inconCount == 0) inconTime = 0;
        else inconTime = inconTime / inconCount;

        // Calculates efficiency
        efficiency = (rawUserData.Count*avgTime) / correct;


        //for (int i = 0; i < rawUserData.Length; i++)
        //{
        //    RawData frameRawData = rawUserData[i];
        //    if (frameRawData.leftOrigin == true && responseTime == null)
        //    {
        //        responseTime = frameRawData.time - startTime;
        //    }
        //    if (frameRawData.leftOrigin == true && sakadTimeState == 0 && sakadTime == null)
        //    {
        //        sakadTimeState = 1;
        //    }
        //    if (frameRawData.enteredOrigin == true && sakadTimeState == 1)
        //    {
        //        sakadTime = frameRawData.time - startTime;
        //    }
        //}

    } //packageData()
}

[System.Serializable]
class RawData
{
    public string shownArrows;
    public bool isCorrect;
    public bool completedTrial;
    public DateTime startTime;
    public DateTime leftOrigin;
    public DateTime hitTarget;
    public DateTime leftTarget;
    public DateTime returnedToOrigin;

}

//[System.Serializable]
//class RawData
//{
//    public Vector3 cameraPosition;
//    public bool leftOrigin;
//    public bool hitTarget;
//    public bool leftTargetHit;
//    public bool enteredOrigin;
//    // states
//    public TimeSpan time;

//    public RawData ()
//    {
//        cameraPosition = GameObject.Find("ARCamera").GetComponent<Vector3>();
//        leftOrigin = (cameraPosition.x > -.05f && cameraPosition.x < .05f && cameraPosition.y > -.05f && cameraPosition.y < .05f);
//        hitTarget = ((cameraPosition.x < -.15f) && (cameraPosition.y < -.3f || cameraPosition.y > .3f));
//        leftTargetHit = (hitTarget && ((cameraPosition.x > -.15f) && (cameraPosition.y > -.3f || cameraPosition.y < .3f)));
//        enteredOrigin = (cameraPosition.x < -.05f && cameraPosition.x > .05f && cameraPosition.y < -.05f && cameraPosition.y > .05f);
//        time = new TimeSpan(DateTime.Now.Ticks);
//    }

//}

