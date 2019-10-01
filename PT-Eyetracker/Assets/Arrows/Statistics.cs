﻿using System.Collections;
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
class TrialData : MonoBehaviour
{
    // Average Reaction Time (milliseconds): Elapsed time between appearance of visual stimulus (set of 5 arrows) and pupil movement to peripheral visual target (X located at least 30 degrees lateral to center fixation point)
    public float avgReactionTime;

    // Proportion of Correct Responses: Number of pupil movements in direction corresponding to direction indicated by center arrow of 5-arrow sets (Number Correct / Total Number of Trials)
    public float proportionOfCorrectResponses;

    // Efficiency Index: Average Reaction Time / Proportion of Correct Responses
    public float efficiencyIndex;

    // Average Reaction Time for Correct Incongruent Responses(<<><< or >><>>)
    public float avgInconReactionTime;

    // Average Reaction Time for Correct Congruent Responses (<<<<< or >>>>>)
    public float avgConReactionTime;

    // Conflict Effect (milliseconds): Average Reaction Time for Correct Incongruent Responses minus Average Reaction Time for Correct Congruent Responses (<<<<< or >>>>>)
    public float conflictEffect;

    // Average Reaction Time for Correct Responses(Incongruent or Congruent)
    public float avgReactionTimeCorrectResponses;

    // Number of sets incomplete (ran out of time)
    public float numIncomplete;

    // Time when the test was started
    public String timestamp;

    // Number/ID to track individual partipant
    public static String participantID;

    public List<RawData> rawUserData = new List<RawData>();

    public String packageData()
    {
        int conCount = 0;
        int inconCount = 0;
        int validCount = 0;

        // Iterate over the list of RawData
        for (int i = 0; i < rawUserData.Count; i++)
        {
            if (rawUserData[i].completedTrial)
            {

                TimeSpan reactionTime = rawUserData[i].hitTarget - rawUserData[i].startTime;

                avgReactionTime += reactionTime.Milliseconds;

                if (rawUserData[i].isCorrect)
                {
                    proportionOfCorrectResponses++;
                    avgReactionTimeCorrectResponses += reactionTime.Milliseconds;

                    if (rawUserData[i].shownArrows == "<<<<<" || rawUserData[i].shownArrows == ">>>>>")
                    {
                        avgConReactionTime += reactionTime.Milliseconds;
                        conCount++;
                    }
                    else
                    {
                        avgInconReactionTime += reactionTime.Milliseconds;
                        inconCount++;
                    }
                    
                }
                validCount++;
            }

        }

        // Calculate the Average reaction time
        avgReactionTime = avgReactionTime / validCount;

        // Calculating the average reaction time of correct responses
        avgReactionTimeCorrectResponses = avgReactionTimeCorrectResponses / proportionOfCorrectResponses;

        // Calculating the proportion of correct responses
        proportionOfCorrectResponses = proportionOfCorrectResponses / validCount;

        // Calculating the efficiency index
        efficiencyIndex = avgReactionTime / proportionOfCorrectResponses;

        // Calculating the average congruent reaction time
        avgConReactionTime = avgConReactionTime / conCount;

        // Caclating the average incongruent reaction time
        avgInconReactionTime = avgInconReactionTime / inconCount;

        // Calculating the conflict effect
        conflictEffect = avgInconReactionTime - avgConReactionTime;        numIncomplete = rawUserData.Count - validCount;        timestamp = TimeZoneInfo.ConvertTime(rawUserData[0].startTime,TimeZoneInfo.Local).ToString();
         
        variable_holder.dataholder[0] = avgReactionTime;
        variable_holder.dataholder[1] = avgConReactionTime;
        variable_holder.dataholder[2] = avgInconReactionTime;
        variable_holder.dataholder[3] = conflictEffect;

        string[] csv_values = new string[] {

            timestamp,

            participantID,

            avgReactionTime.ToString(),
            avgReactionTimeCorrectResponses.ToString(),
            proportionOfCorrectResponses.ToString(),
            efficiencyIndex.ToString(),
            avgConReactionTime.ToString(),
            avgInconReactionTime.ToString(),
            conflictEffect.ToString(),            numIncomplete.ToString()
        };
        string csv_string = "";
        //string csv_string = "timestamp,avgReactionTime,avgReactionTimeCorrectResponses,proportionOfCorrectResponses,efficiencyIndex,avgConReactionTime,avgInconReactionTime,conflictEffect,numIncomplete\n";
        //foreach (string value in csv_values)
        //{            
        //    csv_string += value + ",";
        //}        for (int i = 0; i < csv_values.Length; i++)
        {
            if (i == 0) csv_string += csv_values[i];
            else csv_string += "," + csv_values[i];
        }
        csv_string += "\n";

        return csv_string;


    }

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
    public Vector2 gazeCoords;

}