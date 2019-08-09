using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HSVPicker;

public class changeColor : MonoBehaviour {

    public GameObject camera;
	public Renderer objRenderer;
	public ColorPicker picker;
	// Use this for initialization
	void Start () {

        Camera cam = camera.GetComponent<Camera>();
        
        //objRenderer = gameObject.GetComponent<Renderer>();
		picker.onValueChanged.AddListener(color =>
		{
           // cam.backgroundColor = color;
            objRenderer.material.color = color;

		});
		//cam.backgroundColor = picker.CurrentColor;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
