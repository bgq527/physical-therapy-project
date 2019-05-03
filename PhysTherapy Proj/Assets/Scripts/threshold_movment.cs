using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class threshold_movment : MonoBehaviour {
    Transform[] comparisionTransform;
    Transform[] actualTransform;
    List<string> jointNames = new List<string>()
    {
        "lowerarm_l",
        "lowerleg_r"
    };

    List<string> parentJointNames = new List<string>()
    {
        "upperarm_l",
        "upperleg_r"
    };
    float threshold = 0.1f;
    float scale = 0.2f;
    // Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        comparisionTransform = GameObject.FindGameObjectWithTag("comparision").GetComponentsInChildren<Transform>();
        actualTransform = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("cubes").Count(); i++)
        {

            print(GameObject.FindGameObjectsWithTag("cubes").Count());
            //print(cubeRenderer.Count());
            print(jointNames[i]);

            
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.position = comparisionTransform[GetIndexOfObject(jointNames[i])].transform.position;
            GameObject.FindGameObjectsWithTag("cubes")[i].transform.localScale = new Vector3(scale, scale, scale);
            Vector3 localPosition1 = actualTransform[GetIndexOfObject(jointNames[i])].transform.position - actualTransform[GetIndexOfObject(parentJointNames[i])].transform.position;
            Vector3 localPosition2 = comparisionTransform[GetIndexOfObject(jointNames[i])].transform.position - comparisionTransform[GetIndexOfObject(parentJointNames[i])].transform.position;

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

        

        Vector3 lowerarm_1 = actualTransform[GetIndexOfObject("lowerarm_l")].transform.rotation.eulerAngles;
        Vector3 lowerarm_2 = comparisionTransform[GetIndexOfObject("lowerarm_l")].transform.rotation.eulerAngles;
        


        }
    private int GetIndexOfObject(string name)
    {
        int returnvalue = 0;
        for (int i = 0; i < comparisionTransform.Length; i++)
        {
            if (comparisionTransform[i].name.Equals(name))
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

