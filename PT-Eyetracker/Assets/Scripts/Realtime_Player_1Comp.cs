using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.VR;
using LitJson;
using System;

public class Realtime_Player_1Comp : MonoBehaviour
{
    Transform[] KinectChildObjects;
    Transform[] MovementChildObjects;
    Transform comparisonTag;
    ArrayList JointRotations;
    bool saved = false;
    int index = 0;
    int frame;
    static Vector3 ModelPosition = new Vector3();
    bool rtriggerdown = false;

    ModelQuaternions[] modeljson = new ModelQuaternions[6001];
    bool saveJSON = false;
    bool slerpAll = true;
    bool justxyz = false;
    public static Vector3 saveCurrentPosition;

    Stopwatch stopw;
    List<Vector3> ThisFrameJoints;


    // Use this for initialization
    void Start()
    {
        KinectChildObjects = gameObject.GetComponentsInChildren<Transform>();
        MovementChildObjects = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();
        comparisonTag = GameObject.FindGameObjectWithTag("comparison").GetComponent<Transform>();
        frame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //KinectChildObjects[GetIndexOfObject("root")].transform.localRotation = Quaternion.Euler(0, 180, 0);
        if (BodySourceView.doesBodyExist() == true && ThisFrameJoints == null)
        {
            ThisFrameJoints = BodySourceView.joints;
        }
        if (BodySourceView.doesBodyExist() == true && !ThisFrameJoints.Equals(BodySourceView.joints) && ThisFrameJoints[0] != BodySourceView.joints[0] && ThisFrameJoints != null)
        {
            ThisFrameJoints = BodySourceView.joints;
            if (stopw == null)
            {
                stopw = new Stopwatch();
                stopw.Start();
            }
            //print(frame / stopw.Elapsed.TotalSeconds);
            CurrentFrame cf = MakeChildQuaternionList(ThisFrameJoints);

            CompileFrame(cf.ChildQuaternionList, cf.V3BSVJoints);
            if (saveJSON && frame >= 500 && !saved)
            {
                SaveModelJSON();
            }
            if (!saveJSON && frame >= 1000)
            {
                frame = 0;
            }

            //print(frame);
            frame += 1;

            //KinectChildObjects[GetIndexOfObject("root")].transform.localRotation = Quaternion.Euler(0, 0, 0);

        }

    }

    private void SaveModelJSON()
    {
        if (!justxyz)
        {
            print("JSON saved!");
            saved = true;
            ModelJSON finishedJSON = new ModelJSON();
            finishedJSON.ModelQuaternionList = modeljson;

            string objectToJSON = JsonUtility.ToJson(finishedJSON, true);
            print(objectToJSON);
            //            using (StreamWriter file = new StreamWriter(@"C:\Users\Kinect\Documents\Movements\matchingMovements.json", true))
            using (StreamWriter file = new StreamWriter(@"C:\Users\Kinect\Documents\Movements\"+fileHolder.saveFilename+".json", true))
            {
                file.WriteLine(objectToJSON);
            }
        }
        else
        {
            saved = true;
            ModelJSON finishedJSON = new ModelJSON();
            finishedJSON.ModelQuaternionList = modeljson;

            string objectToJSON = JsonUtility.ToJson(finishedJSON, true);
            print(objectToJSON);
            using (StreamWriter file = new StreamWriter(@"C:\Users\SimWorkstation\Documents\VTaiChi JSONs\ssdfs.csv", true))
            {
                file.WriteLine(objectToJSON);
            }
        }


    }

    private void CompileFrame(List<ChildQuaternion> ChildQuaternionList, List<Vector3> OriginalVector3)
    {
        ModelQuaternions currentFrame = new ModelQuaternions();
        currentFrame.frame = frame;
        currentFrame.ChildQuaternionList = ChildQuaternionList;
        currentFrame.ModelPosition = ModelPosition;
        currentFrame.V3BSVJoints = OriginalVector3;
        modeljson[frame] = currentFrame;
    }

    private CurrentFrame MakeChildQuaternionList(List<Vector3> ThisFrameJoints)
    {
        print(ThisFrameJoints[0]);
        ChildQuaternion leftlowerarm = new ChildQuaternion();
        ChildQuaternion leftupperarm = new ChildQuaternion();
        ChildQuaternion rightlowerarm = new ChildQuaternion();
        ChildQuaternion rightupperarm = new ChildQuaternion();
        ChildQuaternion rightupperleg = new ChildQuaternion();
        ChildQuaternion rightlowerleg = new ChildQuaternion();
        ChildQuaternion leftupperleg = new ChildQuaternion();
        ChildQuaternion leftlowerleg = new ChildQuaternion();
        ChildQuaternion head = new ChildQuaternion();

        //==============#
        //TORSO         #
        //==============#
        //KinectChildObjects[GetIndexOfObject("hip")].transform.rotation = Quaternion.FromToRotation(new Vector3(-1, 0, 1), ThisFrameJoints[0] - ThisFrameJoints[1]);

        // gameObject.transform.position = new Vector3(ThisFrameJoints[0].x, ThisFrameJoints[0].y, ThisFrameJoints[0].z);
        //GameObject.FindGameObjectWithTag("KinectPlayer").transform.position = new Vector3(GameObject.FindGameObjectWithTag("EyeCamera").transform.position.x, GameObject.FindGameObjectWithTag("EyeCamera").transform.position.y - 17, GameObject.FindGameObjectWithTag("EyeCamera").transform.position.z);


        if (frame > 3 && slerpAll)
        {
            ModelQuaternions cqInJSON = modeljson[frame - 1];
            List<ChildQuaternion> currentChildQuaternion = cqInJSON.ChildQuaternionList;
            ChildQuaternion currentJoint = currentChildQuaternion[0];



            //==============#
            //HANDS         #
            //==============#

            leftlowerarm.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[0].quaternion, Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[9] - ThisFrameJoints[10]), 1f);
            leftupperarm.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[1].quaternion, Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[8] - ThisFrameJoints[9]), 1f);
            leftlowerarm.euler = leftlowerarm.quaternion.eulerAngles;
            leftupperarm.euler = leftupperarm.quaternion.eulerAngles;
            //print(leftlowerarm.euler.ToString());
            leftlowerarm.KinectChildObject = "lowerarm_l";
            leftupperarm.KinectChildObject = "upperarm_l";


            rightlowerarm.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[2].quaternion, Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[5] - ThisFrameJoints[6]), 1f);
            rightupperarm.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[3].quaternion, Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[4] - ThisFrameJoints[5]), 1f);
            rightlowerarm.euler = rightlowerarm.quaternion.eulerAngles;
            rightupperarm.euler = rightupperarm.quaternion.eulerAngles;
            rightlowerarm.KinectChildObject = "lowerarm_r";
            rightupperarm.KinectChildObject = "upperarm_r";

            //==============#
            //LEGS          #
            //==============#

            KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(180, 90, -90);
            rightlowerleg.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[4].quaternion, Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[13] - ThisFrameJoints[14]), 1f);
            rightupperleg.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[5].quaternion, Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[12] - ThisFrameJoints[13]), 1f);
            rightlowerleg.euler = rightlowerleg.quaternion.eulerAngles;
            rightupperleg.euler = rightupperleg.quaternion.eulerAngles;
            rightlowerleg.KinectChildObject = "lowerleg_r";
            rightupperleg.KinectChildObject = "upperleg_r";


            KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(180, 90, 90);
            leftlowerleg.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[6].quaternion, Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[17] - ThisFrameJoints[18]), 1f);
            leftupperleg.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[7].quaternion, Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[16] - ThisFrameJoints[17]), 1f);
            leftlowerleg.euler = leftlowerleg.quaternion.eulerAngles;
            leftupperleg.euler = leftupperleg.quaternion.eulerAngles;
            leftlowerleg.KinectChildObject = "lowerleg_l";
            leftupperleg.KinectChildObject = "upperleg_l";

            //==============#
            //HEAD          #
            //==============#

            //head.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[8].quaternion, Quaternion.EulerAngles(Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).z, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).y - 1.5f, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).x - 1.5f), 0.2f);
            head.quaternion = Quaternion.Slerp(modeljson[frame - 1].ChildQuaternionList[8].quaternion, Quaternion.EulerAngles(Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).z, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).y -1.5f, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).x - 1.5f), 0.2f);
            head.euler = head.quaternion.eulerAngles;
            head.KinectChildObject = "head";
        }
        else
        {
            //==============#
            //HANDS         #
            //==============#
            leftlowerarm.quaternion = Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[9] - ThisFrameJoints[10]);
            leftupperarm.quaternion = Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[8] - ThisFrameJoints[9]);
            leftlowerarm.KinectChildObject = "lowerarm_l";
            leftupperarm.KinectChildObject = "upperarm_l";

            rightlowerarm.quaternion = Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[5] - ThisFrameJoints[6]);
            rightupperarm.quaternion = Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[4] - ThisFrameJoints[5]);
            rightlowerarm.KinectChildObject = "lowerarm_r";
            rightupperarm.KinectChildObject = "upperarm_r";

            //==============#
            //LEGS          #
            //==============#
            KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(180, 90, -90);
            rightlowerleg.quaternion = Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[13] - ThisFrameJoints[14]);
            rightupperleg.quaternion = Quaternion.FromToRotation(Vector3.right, ThisFrameJoints[12] - ThisFrameJoints[13]);
            rightlowerleg.KinectChildObject = "lowerleg_r";
            rightupperleg.KinectChildObject = "upperleg_r";

            KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(180, 90, 90);
            leftlowerleg.quaternion = Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[17] - ThisFrameJoints[18]);
            leftupperleg.quaternion = Quaternion.FromToRotation(Vector3.left, ThisFrameJoints[16] - ThisFrameJoints[17]);
            leftlowerleg.KinectChildObject = "lowerleg_l";
            leftupperleg.KinectChildObject = "upperleg_l";

            //==============#
            //HEAD          #
            //==============#
            head.quaternion = Quaternion.EulerAngles(Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).z, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).y - 1.5f, Quaternion.ToEulerAngles(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)).x - 1.5f);
            head.KinectChildObject = "head";
        }

        //==============#
        //APPLYING      #
        //==============#
        KinectChildObjects[GetIndexOfObject("lowerarm_l")].transform.rotation = leftlowerarm.quaternion;
        
            
        KinectChildObjects[GetIndexOfObject("upperarm_l")].transform.rotation = leftupperarm.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerarm_r")].transform.rotation = rightlowerarm.quaternion;
        KinectChildObjects[GetIndexOfObject("upperarm_r")].transform.rotation = rightupperarm.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerleg_r")].transform.rotation = rightlowerleg.quaternion;
        KinectChildObjects[GetIndexOfObject("upperleg_r")].transform.rotation = rightupperleg.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerleg_l")].transform.rotation = leftlowerleg.quaternion;
        KinectChildObjects[GetIndexOfObject("upperleg_l")].transform.rotation = leftupperleg.quaternion;
        KinectChildObjects[GetIndexOfObject("head")].transform.rotation = head.quaternion;


        //==============#
        //FRAME SAVING  #
        //==============#
        List<ChildQuaternion> ChildQuaternionList = new List<ChildQuaternion>();
        ChildQuaternionList.Add(leftlowerarm);
        ChildQuaternionList.Add(leftupperarm);
        ChildQuaternionList.Add(rightlowerarm);
        ChildQuaternionList.Add(rightupperarm);
        ChildQuaternionList.Add(rightlowerleg);
        ChildQuaternionList.Add(rightupperleg);
        ChildQuaternionList.Add(leftlowerleg);
        ChildQuaternionList.Add(leftupperleg);
        ChildQuaternionList.Add(head);

        //==============#
        //EULER ANGLE   #
        //==============#


        CurrentFrame ThisFrame = new CurrentFrame();
        ThisFrame.ChildQuaternionList = ChildQuaternionList;
        ThisFrame.V3BSVJoints = ThisFrameJoints;

        return ThisFrame;

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

    public static void SaveCurrentPosition(Vector3 CurrentPosition)
    {
        ModelPosition = CurrentPosition;
    }
}
