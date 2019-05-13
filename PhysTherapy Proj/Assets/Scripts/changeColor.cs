using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.HSVPicker;

public class changeColor : MonoBehaviour {

	public Renderer objRenderer;
	public ColorPicker picker;
	// Use this for initialization
	void Start () {
		objRenderer = gameObject.GetComponent<Renderer>();
		picker.onValueChanged.AddListener(color =>
		{
			objRenderer.material.color = color;
		});
		objRenderer.material.color = picker.CurrentColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
