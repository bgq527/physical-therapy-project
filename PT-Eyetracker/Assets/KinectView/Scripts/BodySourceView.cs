using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    private static bool bodyExists = false;
    
    public static List<Vector3> joints = new List<Vector3>(new Vector3[0]);
    public static string dirtext;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
        
        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
        
        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    
    void Update () 

    {
        print("Body exists: " + bodyExists.ToString());
        if (BodySourceManager == null)
        {
            return;
        }
        
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }
        
        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
              }
                
            if(body.IsTracked)
            {
                trackedIds.Add (body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        
        // First delete untracked bodies
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach(var body in data)
        {
            if (body == null)
            {
                continue;
            }
            
            if(body.IsTracked)
            {
                if(!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
                bodyExists = true;
            }
        }

    }
    
    public static bool doesBodyExist()
    {
        if (bodyExists == true)
        {
            return true;
        }
        else return false;
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            //jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.transform.localScale = new Vector3(0f, 0f, 0f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        
        return body;
    }


    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        joints = new List<Vector3>(new Vector3[0]);
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            joints.Add(GetVector3FromJoint(sourceJoint));



            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }


            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            if (jt == 0)
            {
                //GameObject.FindGameObjectWithTag("comparison").transform.localPosition = new Vector3(jointObj.localPosition.x, jointObj.localPosition.y, jointObj.localPosition.z);
                //GameObject.FindGameObjectWithTag("comparision").transform.position = new Vector3(GameObject.FindGameObjectWithTag("comparision").transform.position.x, jointObj.localPosition.y, GameObject.FindGameObjectWithTag("comparision").transform.position.z);
                Realtime_Player.SaveCurrentPosition(new Vector3(jointObj.localPosition.x, jointObj.localPosition.y, jointObj.localPosition.z));
            }

            if (jt == Kinect.JointType.FootLeft)
            {
                variable_holder.minPos = jointObj.position.y;
                print(variable_holder.minPos);
                GameObject.Find("FloorCube").transform.position = new Vector3 (0, variable_holder.minPos, 0);
                

            }

            if (jt == Kinect.JointType.FootLeft)
            {
                variable_holder.yvalue = jointObj.transform.position.y;
            }
            //if (jt == 0)
            //{
            //    GameObject.FindGameObjectWithTag("KinectPlayer").transform.position = new Vector3(jointObj.localPosition.x, GameObject.FindGameObjectWithTag("KinectPlayer").transform.position.y, 3.45f - jointObj.localPosition.z);
            //    Realtime_Player_MCS.SaveCurrentPosition(new Vector3(jointObj.localPosition.x, GameObject.FindGameObjectWithTag("MCSRealtime").transform.position.y, jointObj.localPosition.z));
            //}



            //LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            //if (targetJoint.HasValue)
            //{
            //    lr.SetPosition(0, jointObj.localPosition);
            //    lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
            //    lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            //}
            //else
            //{
            //    lr.enabled = false;
            //}
        }
    }    
    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
        case Kinect.TrackingState.Tracked:
            return Color.cyan;

        case Kinect.TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }
}
