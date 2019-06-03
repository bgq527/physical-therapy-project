using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ChildQuaternion
{
    public string KinectChildObject;
    public Quaternion quaternion;
    public Vector3 euler;
}

[Serializable]
public class ModelQuaternions
{
    public int frame;
    public List<Vector3> V3BSVJoints;
    public List<ChildQuaternion> ChildQuaternionList;
    public Vector3 ModelPosition;
}


[Serializable]
public class ModelJSON
{
    public ModelQuaternions[] ModelQuaternionList;
}

[Serializable]
public class CurrentFrame
{
    public List<Vector3> V3BSVJoints;
    public List<ChildQuaternion> ChildQuaternionList;
}
