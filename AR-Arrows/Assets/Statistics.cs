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
    float conTime;
    float inconTime;
    float testTime;
    float avgTime;
    float efficiency;
    int correct;


   public List<RawData> rawUserData;

    public void packageData (int trialNumber)
    {
        DateTime startTime = rawUserData[trialNumber].startTime;
        TimeSpan responseTime = new TimeSpan();
        TimeSpan sakadTime = new TimeSpan();
        int sakadTimeState = 0;


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

