﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class threshold_movment_1_model : MonoBehaviour {
    Transform[] comparisonTransform;
    Transform[] actualTransform;
    Transform[] trainerTransform;
    List<string> jointNames = new List<string>();
    List<string> parentJointNames = new List<string>();

    float threshold = 0.1f;
    float scale = 0.15f;
    // Use this for initialization
	void Start () {
        string[] joint_names_string = File.ReadAllLines(@"C:\Users\Kinect\Documents\Movements\jointlist.csv");

        for (int i = 0; i < joint_names_string.Count(); i++)
        {
            string[] child_and_parent = joint_names_string[i].Split(',');

            jointNames.Add(child_and_parent[0]);
            parentJointNames.Add(child_and_parent[1]);
        }

        for (int i = 0; i < parentJointNames.Count()-1; i++)
        {
            print(i);
            Instantiate(GameObject.FindGameObjectWithTag("cubes"));
        }
        comparisonTransform = GameObject.FindGameObjectWithTag("comparison").GetComponentsInChildren<Transform>();
        actualTransform = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();
        trainerTransform = GameObject.FindGameObjectWithTag("trainer_movement").GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("cubes").Count(); i++)
        {

            print(GameObject.FindGameObjectsWithTag("cubes").Count());
            //print(cubeRenderer.Count());
            print(jointNames[i]);

            
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.position = trainerTransform[GetIndexOfObject(jointNames[i])].transform.position;
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.localScale = new Vector3(scale, scale, scale);
            Vector3 localPosition1 = actualTransform[GetIndexOfObject(jointNames[i])].transform.position - actualTransform[GetIndexOfObject(parentJointNames[i])].transform.position;
            Vector3 localPosition2 = comparisonTransform[GetIndexOfObject(jointNames[i])].transform.position - comparisonTransform[GetIndexOfObject(parentJointNames[i])].transform.position;

            Vector3 localPositions = localPosition1 - localPosition2;
            print(localPositions);

            bool withinPosition = true;
            for (int j = 0; j < 3; j++)
            {
                if (Math.Abs(localPositions[j]) >= threshold)
                {
                    withinPosition = false;
                }
                
            }
            if (withinPosition == true)
            {
                GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().material.color = Color.green;
            } else GameObject.FindGameObjectsWithTag("cubes")[i].GetComponent<Renderer>().material.color = Color.red;

        }
        
        //cubeInformation.Add
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
}


//[Serializable]
//public class Vector3Comparison
//{
//    int threshold;
//    Vector3 joint1;
//    Vector3 joint2;
//    Transform cubeTransform;
//}
