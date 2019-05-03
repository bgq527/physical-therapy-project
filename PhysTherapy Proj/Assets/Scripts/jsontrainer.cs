using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

public class jsontrainer : MonoBehaviour
{
    private string jsonstring;
    private ModelJSON json;
    int currentFrame;
    Transform[] KinectChildObjects;
    ChildQuaternion leftlowerarm = new ChildQuaternion();
    ChildQuaternion leftupperarm = new ChildQuaternion();
    ChildQuaternion rightlowerarm = new ChildQuaternion();
    ChildQuaternion rightupperarm = new ChildQuaternion();
    ChildQuaternion rightupperleg = new ChildQuaternion();
    ChildQuaternion rightlowerleg = new ChildQuaternion();
    ChildQuaternion leftupperleg = new ChildQuaternion();
    ChildQuaternion leftlowerleg = new ChildQuaternion();
    ChildQuaternion head = new ChildQuaternion();

    ModelQuaternions[] mqInJSON;
    ModelQuaternions cqInJSON;
    // Use this for initialization

    void Start()
    {
        KinectChildObjects = gameObject.GetComponentsInChildren<Transform>();
        currentFrame = 0;
        string reader = File.ReadAllText(@"C:\Users\Kinect\Documents\Movements\movementFile.json");
        json = JsonMapper.ToObject<ModelJSON>(reader);
        mqInJSON = json.ModelQuaternionList;
        
    }

    IEnumerator Play_JSON()
    {
        gameObject.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        cqInJSON = mqInJSON[currentFrame];
        List<ChildQuaternion> currentChildQuaternion = cqInJSON.ChildQuaternionList;
        Vector3 localPosition = cqInJSON.ModelPosition;

        //Debug.Log(currentChildQuaternion[0]);

        leftlowerarm.quaternion = currentChildQuaternion[0].quaternion;
        leftupperarm.quaternion = currentChildQuaternion[1].quaternion;
        rightlowerarm.quaternion = currentChildQuaternion[2].quaternion;
        rightupperarm.quaternion = currentChildQuaternion[3].quaternion;
        rightlowerleg.quaternion = currentChildQuaternion[4].quaternion;
        rightupperleg.quaternion = currentChildQuaternion[5].quaternion;
        leftlowerleg.quaternion = currentChildQuaternion[6].quaternion;
        leftupperleg.quaternion = currentChildQuaternion[7].quaternion;
        head.quaternion = currentChildQuaternion[8].quaternion;

        KinectChildObjects[GetIndexOfObject("lowerarm_l")].transform.rotation = leftlowerarm.quaternion;
        KinectChildObjects[GetIndexOfObject("upperarm_l")].transform.rotation = leftupperarm.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerarm_r")].transform.rotation = rightlowerarm.quaternion;
        KinectChildObjects[GetIndexOfObject("upperarm_r")].transform.rotation = rightupperarm.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerleg_r")].transform.rotation = rightlowerleg.quaternion;
        KinectChildObjects[GetIndexOfObject("upperleg_r")].transform.rotation = rightupperleg.quaternion;
        KinectChildObjects[GetIndexOfObject("lowerleg_l")].transform.rotation = leftlowerleg.quaternion;
        KinectChildObjects[GetIndexOfObject("upperleg_l")].transform.rotation = leftupperleg.quaternion;

        KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(180, 90, -90);
        KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(180, 90, 90);

        transform.parent.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);


        yield return new WaitForSeconds(0.60f);

        if (currentFrame > 500) currentFrame = 0;
        else currentFrame++;
    }

    // Update is called once per frame
    void Update()
    { 
        StartCoroutine(Play_JSON());
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

    /*
    private Quaternion getQuaternion(string KinectChildObject, int frame)
    {
        double dx = (double)json["ModelQuaternionList"][frame]["ChildQuaternionList"][KinectChildObject]["quaternion"]["x"];
        double dy = (double)json["ModelQuaternionList"][frame]["ChildQuaternionList"][KinectChildObject]["quaternion"]["y"];
        double dz = (double)json["ModelQuaternionList"][frame]["ChildQuaternionList"][KinectChildObject]["quaternion"]["z"];
        double dw = (double)json["ModelQuaternionList"][frame]["ChildQuaternionList"][KinectChildObject]["quaternion"]["w"];

        float x = (float)dx;
        float y = (float)dy;
        float z = (float)dz;
        float w = (float)dw;

        Quaternion orientation = new Quaternion(x, y, z, w);
        return orientation;
    }
    */
    

}
