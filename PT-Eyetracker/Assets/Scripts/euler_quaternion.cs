using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class euler_quaternion : MonoBehaviour {
    private string jsonstring;
    private ModelJSON json;
    int currentFrame;
    List<Quaternion> leftlowerarm = new List<Quaternion>();
    List<Quaternion> leftupperarm = new List<Quaternion>();
    List<Quaternion> rightlowerarm = new List<Quaternion>();
    List<Quaternion> rightupperarm = new List<Quaternion>();
    List<Quaternion> rightupperleg = new List<Quaternion>();
    List<Quaternion> rightlowerleg = new List<Quaternion>();
    List<Quaternion> leftupperleg = new List<Quaternion>();
    List<Quaternion> leftlowerleg = new List<Quaternion>();
    List<Quaternion> head = new List<Quaternion>();
    int frame = 0;

    Transform[] KinectChildObjects;
    // Use this for initialization
    void Start () {
        KinectChildObjects = gameObject.GetComponentsInChildren<Transform>();
        //var training_data_file = Resources.Load("test_3_training") as TextAsset;
        //var joint_data_file = Resources.Load("result") as TextAsset;
        string[] training_data_string = File.ReadAllLines(@"C:\Users\SimWorkstation\Documents\CSV\test_3_training.csv");
        string[] joint_data_string = File.ReadAllLines(@"C:\Users\SimWorkstation\Documents\CSV\result.csv");
        for (int i = 0; i < 1000; i++)
        {
            
            string[] training_data_frame = training_data_string[i].Split(',');
            leftlowerarm.Add(Quaternion.Euler(float.Parse(training_data_frame[0]), float.Parse(training_data_frame[1]), float.Parse(training_data_frame[2])));
            leftupperarm.Add(Quaternion.Euler(float.Parse(training_data_frame[3]), float.Parse(training_data_frame[4]), float.Parse(training_data_frame[5])));
            rightlowerarm.Add(Quaternion.Euler(float.Parse(training_data_frame[6]), float.Parse(training_data_frame[7]), float.Parse(training_data_frame[8])));
            rightupperarm.Add(Quaternion.Euler(float.Parse(training_data_frame[9]), float.Parse(training_data_frame[10]), float.Parse(training_data_frame[11])));
            rightlowerleg.Add(Quaternion.Euler(float.Parse(training_data_frame[12]), float.Parse(training_data_frame[13]), float.Parse(training_data_frame[14])));
            rightupperleg.Add(Quaternion.Euler(float.Parse(training_data_frame[15]), float.Parse(training_data_frame[16]), float.Parse(training_data_frame[17])));
            leftupperleg.Add(Quaternion.Euler(float.Parse(training_data_frame[18]), float.Parse(training_data_frame[19]), float.Parse(training_data_frame[20])));
            //head.Add(Quaternion.eulerAngle(float.Parse(training_data_frame[6]), float.Parse(training_data_frame[7]), float.Parse(training_data_frame[8])));
            

            string[] joint_data_frame = joint_data_string[i].Split(',');
            if(leftlowerleg.Count > 2)
            {
                leftlowerleg.Add(Quaternion.Slerp(leftlowerleg[i - 1], Quaternion.Euler(float.Parse(joint_data_frame[0]), float.Parse(joint_data_frame[1]), float.Parse(joint_data_frame[2])), 0.2f));
            }
            else
            {
                leftlowerleg.Add(Quaternion.Euler(float.Parse(joint_data_frame[0]), float.Parse(joint_data_frame[1]), float.Parse(joint_data_frame[2])));
            }
        }
        print(leftlowerleg.Count);
    }
	
	// Update is called once per frame
	void Update () {
        KinectChildObjects[GetIndexOfObject("lowerarm_l")].transform.rotation = leftlowerarm[frame];
        KinectChildObjects[GetIndexOfObject("upperarm_l")].transform.rotation = leftupperarm[frame];
        KinectChildObjects[GetIndexOfObject("lowerarm_r")].transform.rotation = rightlowerarm[frame];
        KinectChildObjects[GetIndexOfObject("upperarm_r")].transform.rotation = rightupperarm[frame];
        KinectChildObjects[GetIndexOfObject("lowerleg_r")].transform.rotation = rightlowerleg[frame];
        KinectChildObjects[GetIndexOfObject("upperleg_r")].transform.rotation = rightupperleg[frame];
        KinectChildObjects[GetIndexOfObject("lowerleg_l")].transform.rotation = leftlowerleg[frame];
        //KinectChildObjects[GetIndexOfObject("lowerarm_l")].transform.rotation = head[frame];

        KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(180, 90, -90);
        KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(180, 90, 90);
        KinectChildObjects[GetIndexOfObject("upperleg_l")].transform.rotation = leftupperleg[frame];
        if (frame < 1000)
        {
            frame++;
        }
        else
        {
            frame = 0;
        }

    }
    private int GetIndexOfObject(string name)
    {
        int returnvalue = 0;
        for (int i = 0; i < KinectChildObjects.Length; i++)
        {
            if (KinectChildObjects[i].name.Equals(name))
            {
                returnvalue = i;
            }
        }
        return returnvalue;
    }

}
