using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class threshold_movment_1_model : MonoBehaviour {
    Transform[] comparisonTransform;
    Transform[] actualTransform;
    Transform[] trainerTransform;
    List<string> jointNames = new List<string>();
    List<string> parentJointNames = new List<string>();
    string[] child_and_parent;

    // 0: Left knee, 1: Right knee, 2: Left Pelvis, 3: Right Pelvis, 4: Trunk
    public static float[] thresholds = { .15f, .15f, .2f, .2f, .05f, .05f};

    // 0: No movement/other, 1: left squat, 2: right squat, 3: unsupported file, 4: no markers enabled
    public static int currentMovement = 0;

    bool[,] possiblejoints = {
            {
                true,true,true,true,true,true
            },
            {
                true,false,false,true,true,true
            },
            {
                false,true,true,false, true,true
            },
            {
                false,false,false,false,false,false
            }
        };

    String[] textNames =
    {
        "LeftKneeText","RightKneeText","LeftHipText","RightHipText","LeftShoulderText","RightShoulderText"
    };





    // Use this for initialization
    void Start () {

        
        //string[] joint_names_string = File.ReadAllLines(@"C:\Users\Kinect\Documents\Movements\jointlist.csv");
        string[] joint_names_string = 

       
        // Add this line after the equals part of the string[] line
        Regex.Split(Resources.Load<TextAsset>("CSVs/jointlist").ToString(), "\n");

        

        for (int i = 0; i < joint_names_string.Count(); i++)
        {
            child_and_parent = joint_names_string[i].Split(',');
            jointNames.Add(child_and_parent[0]);
            parentJointNames.Add(child_and_parent[1]);
        }

        print(jointNames.Count);
        print(parentJointNames.Count);
        for (int i = 0; i < parentJointNames.Count()-1; i++)
        {
            print(i);
         //   Instantiate(GameObject.FindGameObjectWithTag("cubes"));
            var temp = Instantiate(GameObject.Find("BlobLightProjector"));
            temp.name = jointNames[i+1];
        //    GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().material.color = Color.green;
            GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().material.color = Color.green;
        }


        comparisonTransform = GameObject.FindGameObjectWithTag("comparison").GetComponentsInChildren<Transform>();
        actualTransform = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();
        trainerTransform = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        float threshold = fileHolder.threshold;
        float scale = fileHolder.scale;

        int[] order = { 1, 0, 3, 2, 4, 5};

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("proj").Count(); i++ )
        {
            //    print(GameObject.FindGameObjectsWithTag("proj").Count());
            if (possiblejoints[currentMovement,i] == true && currentMovement != 0)
            {

                // Find the projector assigned to the current joint and update its position to match the joints actual location
                var pos = trainerTransform[GetIndexOfObject(jointNames[i])].transform.position;
                pos.z -= 1;
                GameObject.FindGameObjectsWithTag("proj")[i].transform.position = pos;

                // obsolete
                Vector3 localPosition1 = actualTransform[GetIndexOfObject(jointNames[order[i]])].transform.position - actualTransform[GetIndexOfObject(parentJointNames[order[i]])].transform.position;
                Vector3 localPosition2 = comparisonTransform[GetIndexOfObject(jointNames[order[i]])].transform.position - comparisonTransform[GetIndexOfObject(parentJointNames[order[i]])].transform.position;
                Vector3 localPositions = localPosition1 - localPosition2;
                //print(jointNames[i]+": "+localPositions);

                Vector3 localPosition3 = actualTransform[GetIndexOfObject(jointNames[order[i]])].transform.position;
                Vector3 localPosition4 = comparisonTransform[GetIndexOfObject(jointNames[order[i]])].transform.position;

                
                //print("Child only "+jointNames[i] + ": " + Vector3.Angle(localPosition3, localPosition4));

                //print("Child and Parent " + jointNames[i] + ": " + Vector3.Angle(localPosition2, localPosition1));

                bool withinPosition = true;

                //float angle = Vector3.Angle(localPosition1, localPosition2);

                //withinPosition = (angle <= thresholds[i]);

                

                for (int j = 0; j < 3; j++)
                {

                    if (Math.Abs(localPositions[j]) >= thresholds[i])
                    {
                        withinPosition = false;
                    }

                }

                // DEBUG TEXT
                //var text = GameObject.Find(textNames[i]).GetComponent<Text>();
                //text.text = jointNames[i] + " withinPos: " + withinPosition + ", Angle: " + localPositions.ToString();


                if (withinPosition == true /* && variable_holder.calibrated == true*/)
                {
                    //GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().enabled = true;
                    //GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Projector>().enabled = true;
                    GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().enabled = true;

                }
                else
                {
                    //GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<MeshRenderer>().enabled = false;
                    GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().enabled = false;
                    //GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<MeshRenderer>().material.color = Color.red;

                }
                
            }

            // Make sure the projectors clear when they are no longer being used. 
            if (possiblejoints[currentMovement, i] == false && GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().enabled) GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().enabled = false;
        }

    }
    private int GetIndexOfObject(string name)
    {
        int returnvalue = 0;
        for (int i = 0; i < trainerTransform.Length; i++)
        {
            if (trainerTransform[i].name.Equals(name))
            {
                returnvalue = i;
            }
        }
        return returnvalue;
    }

    public static void ChangeMovement()
    {
        var GO = GameObject.Find("MarkerToggle");
        bool isEnabled = GO.GetComponent<Toggle>().isOn;

        // for in unity editor/any computer
        //int dirchar = 7;
        // For mapp computer:
        int dirchar = 33;

        string filename = fileHolder.movementFilename.Substring(Math.Max(0, fileHolder.movementFilename.Length - (fileHolder.movementFilename.Length - dirchar)));

        if (isEnabled)
        {
            if (filename.Equals("squatleft.json")) currentMovement = 1;
            else if (filename.Equals("squatright.json")) currentMovement = 2;
            else currentMovement = 3;

        }

        else
        {
            currentMovement = 4;
            ClearMarkers();
        }
        print(currentMovement+": " + filename);
        var text = GameObject.Find("MarkerInfoText").GetComponent<Text>();
        text.text = "Displaying markers for: " + filename;
        if (currentMovement == 3) text.text = "Displaying markers for: Unsupported movement file";
        if (currentMovement == 4) text.text = "Displaying markers for: Markers disabled";
    }

    static void ClearMarkers()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject.FindGameObjectsWithTag("proj")[i].GetComponent<Projector>().enabled = false;
        }
    }

    // Method to change the Joint Thresholds
    public static void EditJointThresholds(string joint, float thresholdValue)
    {
        if (joint == "knee")
        {
            thresholds[0] = thresholdValue;
            thresholds[1] = thresholdValue;
        }

        if (joint == "hip")
        {
            thresholds[2] = thresholdValue;
            thresholds[3] = thresholdValue;
        }

    }
}


//[Serializable]
//public class Vector3Comparison
//{
//    int threshold;
//    Vector3 joint1;
//    Vector3 joint2;
//    Transform cubeTransform;
//}

