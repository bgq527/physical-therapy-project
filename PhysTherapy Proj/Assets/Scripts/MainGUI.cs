using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGUI : MonoBehaviour {

	public Rect window = new Rect(15, 15, 250, 250);

	private void OnGUI(){
		window = GUI.Window(0, window, mainGUIWindow, "Options");
	}

	private void mainGUIWindow(int windowID){
		GUI.Button(new Rect(15, 15, 50, 50), "Button");

		GUI.DragWindow();
	}
}
