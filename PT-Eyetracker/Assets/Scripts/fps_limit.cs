using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_limit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
