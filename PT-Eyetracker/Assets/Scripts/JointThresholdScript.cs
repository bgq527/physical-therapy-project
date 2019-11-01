using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointThresholdScript : MonoBehaviour
{
    GameObject ThresholdCanvas;

    private void Start()
    {
        InputField IFh = GameObject.Find("HipThresholdInputField").GetComponent<InputField>();
        InputField IFk = GameObject.Find("KneeThresholdInputField").GetComponent<InputField>();

        IFh.text = threshold_movment_1_model.thresholds[2] + "";
        IFk.text = threshold_movment_1_model.thresholds[0] + "";

        ThresholdCanvas = GameObject.Find("ThresholdCanvas");

        ThresholdCanvas.active = false;



    }

    public void EditHipThreshold()
    {
        InputField IF = GameObject.Find("HipThresholdInputField").GetComponent<InputField>();
        float threshold = float.Parse(IF.text);

        print("Changing Hip threshold to " + threshold * 100 + " degrees.");
        threshold_movment_1_model.EditJointThresholds("hip", threshold);
    }

    public void EditKneeThreshold()
    {
        InputField IF = GameObject.Find("KneeThresholdInputField").GetComponent<InputField>();
        float threshold = float.Parse(IF.text);

        print("Changing knee threshold to " + threshold * 100 + " degrees.");
        threshold_movment_1_model.EditJointThresholds("knee", threshold);
    }

    public void ChangeThresholdSettingsVisibility()
    {
        ThresholdCanvas.active = !ThresholdCanvas.active;
    }
}