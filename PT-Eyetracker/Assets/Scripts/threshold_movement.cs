using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class threshold_movement : MonoBehaviour {
	Transform[] comparisonTransform;
    Transform[] actualTransform;
    Transform[] trainerTransform;
    List<string> jointNames = new List<string>();
    List<string> parentJointNames = new List<string>();

    float threshold = 0.1f;
    float scale = 0.15f;
	// Use this for initialization
	void Start () {
		comparisonTransform = GameObject.Find("actual_model").GetComponentsInChildren<Transform>();
		actualTransform = GameObject.Find("movement_model_comp").GetComponentsInChildren<Transform>();
		trainerTransform = GameObject.Find("movement_model").GetComponentsInChildren<Transform>();

		string[] joint_names_string = Regex.Split(Resources.Load<TextAsset>("CSVs/jointlist").ToString(), "\n|\r|\r\n");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
