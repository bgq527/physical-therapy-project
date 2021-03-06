﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class jsontrainer : MonoBehaviour
{
    // Used for moving the floor
    float lowest_foot;

    private string jsonstring;
    public static bool loaded;
    private ModelJSON json;
    int currentFrame;
    Transform[] KinectChildObjects;
    Transform[] MovementChildObjects;
    ChildQuaternion leftlowerarm = new ChildQuaternion();
    ChildQuaternion leftupperarm = new ChildQuaternion();
    ChildQuaternion rightlowerarm = new ChildQuaternion();
    ChildQuaternion rightupperarm = new ChildQuaternion();
    ChildQuaternion rightupperleg = new ChildQuaternion();
    ChildQuaternion rightlowerleg = new ChildQuaternion();
    ChildQuaternion leftupperleg = new ChildQuaternion();
    ChildQuaternion leftlowerleg = new ChildQuaternion();
    ChildQuaternion head = new ChildQuaternion();
    SkinnedMeshRenderer movementMeshRenderer;
    private float timer = 0.0f;
    bool play = false;
    float timer2 = 0f;
    int count = 0;
    int lastframe=0;
    public static int replayCount = 0;
    DateTime startTime;
    DateTime currentTime;

    ModelQuaternions[] mqInJSON;
    ModelQuaternions cqInJSON;
    // Use this for initialization

    void Start()
    {
        loaded = false;
        KinectChildObjects = gameObject.GetComponentsInChildren<Transform>();
        //foreach (Transform trans in KinectChildObjects) print(trans);
        currentFrame = 0;
        movementMeshRenderer = GameObject.FindGameObjectWithTag("movementmr").GetComponent<SkinnedMeshRenderer>();
        

    }

    IEnumerator Play_JSON()
    {
       // gameObject.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        
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

       // KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(0, 0, 0);
       // KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(0, 0, 0);

        KinectChildObjects[GetIndexOfObject("foot_r")].transform.rotation = Quaternion.Euler(180, 90, -90);
        KinectChildObjects[GetIndexOfObject("foot_l")].transform.rotation = Quaternion.Euler(180, 90, 90);

        MovementChildObjects[GetIndexOfMovementObject("root")].rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
        //ransform.parent.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);

        if (KinectChildObjects[GetIndexOfObject("foot_r")].transform.position.y > KinectChildObjects[GetIndexOfObject("foot_l")].transform.position.y) {
            lowest_foot = KinectChildObjects[GetIndexOfObject("foot_l")].transform.position[1];
        }
        else
        {
            lowest_foot = KinectChildObjects[GetIndexOfObject("foot_r")].transform.position[1];
        }

        float floor_y = GameObject.FindGameObjectWithTag("floorcube").transform.position.y;
        float difference = lowest_foot - floor_y;
        float new_model_y = gameObject.transform.position.y - difference + 0.2f;
        fileHolder.new_model_y = new_model_y;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, new_model_y, gameObject.transform.position.z);

        yield return new WaitForSeconds(0.60f);

        if (currentFrame > mqInJSON.Length) currentFrame = 0;
        else currentFrame++;
        currentTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            Text time = GameObject.Find("TimeText").GetComponent<Text>();
            time.text = currentTime - startTime + "";
            Text count = GameObject.Find("CountText").GetComponent<Text>();
            count.text = replayCount+"" ;
        }

        if (currentFrame < lastframe) replayCount++;

        lastframe = currentFrame - 1;

        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        //print(timer);

        if (timer >= .0410f)
        {
            play = true;
            timer = 0f;
           // count++;
        }


        // How many times a second the movements are being played
        // Should be 24..
        /*
        if (timer2 >= 1f)
        {
            print(count);
            count = 0;
            timer2 = 0f;
        }
        */

        else play = false;

        //if (variable_holder.calibrated == true)
       // {
       //     movementMeshRenderer.enabled = true;
            if (!loaded)
            {
                if (fileHolder.movementFilename != "")
                {
                    string reader = File.ReadAllText(fileHolder.movementFilename);
                    json = JsonMapper.ToObject<ModelJSON>(reader);
                    mqInJSON = json.ModelQuaternionList;
                    MovementChildObjects = GameObject.FindGameObjectWithTag("movement").GetComponentsInChildren<Transform>();
                    loaded = true;
                    startTime = DateTime.Now;
                    threshold_movment_1_model.ChangeMovement();
                }
            }
            else if (loaded && play)
            {
                StartCoroutine(Play_JSON());
                
            }
        //}
        //else
       // {
       //     movementMeshRenderer.enabled = false;
       // }
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

    private int GetIndexOfMovementObject(string name)
    {
        int returnvalue = 0;
        for (int i = 0; i < MovementChildObjects.Length; i++)
        {
            if (MovementChildObjects[i].name.Equals(name))
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
