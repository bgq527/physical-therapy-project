using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    Text leftTarget;
    Text rightTarget;
    public GameObject RightConfText;
    public GameObject NextSetText;
    public GameObject LineRenderObject;
    public static bool showThresholds;
    public static int hitmarkerPointsToDraw = 4;


    // Start is called before the first frame update
    void Start()
    {
        // GameObject component instantiation
        leftTarget = GameObject.Find("Left").GetComponent<Text>();
        rightTarget = GameObject.Find("Right").GetComponent<Text>();
        leftTarget.text = "☐";
        rightTarget.text = "☐";
        showThresholds = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();

        if (Calibration.showThresholds)
        {
            
            leftTarget.text = "☐";
            rightTarget.text = "☐";

        }
        DrawEyeHitMarker(variable_holder.eyeRotation.x, variable_holder.eyeRotation.y);
        if (!Calibration.showThresholds)
        {

            leftTarget.text = "";
            rightTarget.text = "";
        }

        
    }


    

    // Updates the stats box
    void UpdateStats()
    {
        Text rightText = RightConfText.GetComponent<Text>();
        Text nextText = NextSetText.GetComponent<Text>();

        rightText.text = variable_holder.conf+"";
        nextText.text = ((CheckHitTarget(variable_holder.eyeRotation.x, variable_holder.eyeRotation.y)))+"";
    }

    
    private bool CheckHitTarget(float x, float y)
    {
        if ((x > .3f || x < -.3f) && (y > -.05f || y < .05f)) return true;
        else return false;
    }

    // Draws hit marker where eye gaze is looking
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

    

   

}
