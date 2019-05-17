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
    // average of sakadTime when the arrows shown were congruent
    public float conTime;

    // average of sakadTime when the arrows shown were incongruent
    public float inconTime;

    // max time that they took to hit the target sakadTime
    public float testTime;

    // average of response time 
    public float avgTime;

    // ( numTrials * average response ) / correct answers
    public float efficiency;

    // count of correct answers (out of 12)
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
            responseTime = rawUserData[i].returnedToOrigin - rawUserData[i].leftTarget;

            // Sakad time calculated as the time that it took to go from the origin to the target
            // sakadTime = rawUserData[i].hitTarget - rawUserData[i].leftOrigin;
            sakadTime = rawUserData[i].hitTarget - rawUserData[i].startTime;

            // Determines if the arrows are congruent or incongruent
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
            
            // Counts the number of correct answers
            if (rawUserData[i].isCorrect == true)
            {
                correct++;
            }

            // Determines the longest response time, which is the "testTime"
            if (responseTime.Milliseconds > testTime)
            {
                testTime = responseTime.Milliseconds;
            }

            avgTime += sakadTime.Milliseconds;
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