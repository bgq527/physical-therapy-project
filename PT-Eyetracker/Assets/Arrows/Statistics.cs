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

    public List<RawData> rawUserData = new List<RawData>();

    public void packageData()
    {
        int conCount = 0;
        int inconCount = 0;
        for (int i = 0; i < rawUserData.Count; i++)
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
            
        }

        avgReactionTime = avgReactionTime / rawUserData.Count;
        avgReactionTimeCorrectResponses = avgReactionTimeCorrectResponses / proportionOfCorrectResponses;
        proportionOfCorrectResponses = proportionOfCorrectResponses / rawUserData.Count;
        efficiencyIndex = avgReactionTime / proportionOfCorrectResponses;
        avgConReactionTime = avgConReactionTime / conCount;
        avgInconReactionTime = avgInconReactionTime / inconCount;
        conflictEffect = avgInconReactionTime - avgConReactionTime;

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

}