using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThreshhold : MonoBehaviour {
    Transform cameraTransform;

	// Use this for initialization
	void Start () {
        cameraTransform = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        float cameraX = cameraTransform.transform.rotation.x;
        float cameraY = cameraTransform.transform.rotation.y;

        if ((cameraX < -30 && cameraY < -50) || (cameraX < -30 && cameraY > 50))
        {

        } 
	}
}
