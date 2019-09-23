using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public GameObject ShowOptions;
    public GameObject ShowThresholds;
    public GameObject DisappearInputField;
    public GameObject EndSetInputField;
    public GameObject OriginSizeInputField;
    public GameObject TargetXInputField;
    public GameObject TargetScaleInputField;
    private Arrows arrows;

    void Start()
    {
        arrows = new Arrows();

        DisappearInputField.GetComponentInChildren<Text>().text = Arrows.arrowTiming+"";
        EndSetInputField.GetComponentInChildren<Text>().text = Arrows.setTiming*1000 + "";
        OriginSizeInputField.GetComponentInChildren<Text>().text = Arrows.originThreshold*100+"";
    }

    // Change whether to show the threshold and pupil gaze
    public void ShowThresholdsClicked()
    {
        if (!arrows.showThresholds)
        {
            Arrows.thresholdPointsToDraw = 0;
            Arrows.hitmarkerPointsToDraw = 0;

        }
        else
        {
            Arrows.thresholdPointsToDraw = 30;
            Arrows.hitmarkerPointsToDraw = 4;
            Debug.Log("Threshold visualizations turned on!");
        }
        arrows.showThresholds = !arrows.showThresholds;
    }

    // Change the timing for when the arrows disappear 
    public void ArrowTiming(int milliseconds)
    {
        InputField IF = DisappearInputField.GetComponent<InputField>();
        float flo = float.Parse(IF.text);
        Arrows.setTiming = (int)flo;
        Debug.Log("Setting the Arrow timing to " + Arrows.arrowTiming + " milliseconds");
    }
    
    // Change the timing for when the set changes automatically 
    public void SetTiming(int milliseconds)
    {
        InputField IF = EndSetInputField.GetComponent<InputField>();
        float flo = float.Parse(IF.text) / 1000;
        Arrows.setTiming = flo;
        Debug.Log("Setting the set timing to "+Arrows.setTiming + " seconds");
    }

    // Change the threshold size
    public void ThresholdSize()
    {
        InputField IF = OriginSizeInputField.GetComponent<InputField>();
        float deg = float.Parse(IF.text);
        Arrows.originThreshold = deg/100;
        Debug.Log("Setting the origin threshold to "+ deg + " degrees.");
    }

    // Change the location of the Targets
    public void MoveTargets()
    {
        InputField IFx = TargetXInputField.GetComponent<InputField>();
        
        // xCoord represents the x value of the closest line to the center (i.e. on a target on the left side of the screen the right most line on the square)
        float xCoord = float.Parse(IFx.text);
        // yCoord represents the y value of the center of the target
        

        Arrows.targetX = xCoord;



    }

    public void ScaleTargets()
    {
        InputField IF = TargetScaleInputField.GetComponent<InputField>();


    }

}
