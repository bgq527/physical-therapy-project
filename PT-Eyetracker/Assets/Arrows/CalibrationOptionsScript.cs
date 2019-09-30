using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationOptionsScript : MonoBehaviour
{
    public GameObject ShowOptions;
    public GameObject ShowThresholds;
    public GameObject DisappearInputField;
    public GameObject EndSetInputField;
    public GameObject OriginSizeInputField;
    public GameObject TargetXInputField;
    public GameObject TargetScaleInputField;
    public GameObject OptionsCanvas;

    

    // Change whether to show the threshold and pupil gaze
    public void ShowThresholdsClicked()
    {
        

        Calibration.showThresholds = !Calibration.showThresholds;
        print(Calibration.showThresholds);

        if (!Calibration.showThresholds)
        {
            Calibration.hitmarkerPointsToDraw = 0;

        }
        else
        {
            Calibration.hitmarkerPointsToDraw = 4;
            Debug.Log("Threshold visualizations turned on!" + Calibration.hitmarkerPointsToDraw);
        }
    }

    

}
