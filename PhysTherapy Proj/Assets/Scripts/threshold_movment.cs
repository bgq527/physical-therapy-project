using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class threshold_movment : MonoBehaviour {
    Transform[] comparisonTransform;
    Transform[] actualTransform;

    // Stores names of joints for accuracy thresholding
    List<string> jointNames = new List<string>();
    // Stores names of parents of joints for accuracy thresholding
    List<string> parentJointNames = new List<string>();

    // Threshold for classifying movement as matching the example or not. Model coordinate system (not real-world coords)
    
    // Use this for initialization
	void Start () {
        // Parse CSV for list of joints to compare to example (Change to GUI eventually)
        //string[] joint_names_string = File.ReadAllLines(@"C:\Users\Kinect\Documents\Movements\jointlist.csv");
        string[] joint_names_string = Regex.Split(Resources.Load("jointlist").ToString(), "\n|\r|\r\n");
        for (int i = 0; i < joint_names_string.Count(); i++)
        {
            string[] child_and_parent = joint_names_string[i].Split(',');
            // Parent joints and child joints sent to appropriate lists
            jointNames.Add(child_and_parent[0]);
            parentJointNames.Add(child_and_parent[1]);
        }

        // Copy first cube to add colorable game objects to show match/error status (One cube already puplated)
        for (int i = 0; i < parentJointNames.Count()-1; i++)
        {
            Instantiate(GameObject.FindGameObjectWithTag("cubes"));
        }
    }
	
	// Update is called once per frame
	void Update () {
        float threshold = fileHolder.threshold;
        float scale = fileHolder.scale;
        // Get object transforms for objects being compared
        comparisonTransform = GameObject.FindGameObjectWithTag("comparison").GetComponentsInChildren<Transform>();
        actualTransform = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();

        // Iterate over objects being compared (equals number of cubes used as labels)
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("cubes").Count(); i++)
        {
            // Move cube to appropriate location (location of child joint)
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.position = comparisonTransform[GetIndexOfObject(jointNames[i])].transform.position;

            // Assign cube's scale (i.e. size, vector 3d (could adjust dims separately))
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.localScale = new Vector3(scale, scale, scale);

            // Get subject's parent and child positions, calculate displacement
            Vector3 localPosition1 = actualTransform[GetIndexOfObject(jointNames[i])].transform.position - actualTransform[GetIndexOfObject(parentJointNames[i])].transform.position;
            // Get model's parent and child positions (ideal positions), calculate displacement
            Vector3 localPosition2 = comparisonTransform[GetIndexOfObject(jointNames[i])].transform.position - comparisonTransform[GetIndexOfObject(parentJointNames[i])].transform.position;

            // Calculate error in subject's position vs comparison model's position
            Vector3 localPositionErr = localPosition1 - localPosition2;

            // If joint position is off by a distance less than or equal to threshhold, color cube green, else green
            if (Math.Sqrt(Math.Pow(localPositionErr[1],2) + Math.Pow(localPositionErr[2],2) + Math.Pow(localPositionErr[3],2)) <= threshold)
            {
                GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().material.color = Color.green;
            }else GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private int GetIndexOfObject(string name)
    {
        int returnvalue = 0;
        for (int i = 0; i < comparisonTransform.Length; i++)
        {
            if (comparisonTransform[i].name.Equals(name))
            {
                returnvalue = i;
            }
        }
        return returnvalue;
    }
}