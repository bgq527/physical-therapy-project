using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using System.Runtime.InteropServices;

public class optionsGUI : MonoBehaviour {
	public int margin = 15;
	private Rect statsWindow = new Rect(Screen.width - 265, 15, 250, 70);
	private Rect loadMovementWindow = new Rect(Screen.width - 265, 85, 250, 70);
	

	[DllImport("user32.dll")]
   private static extern void OpenFileDialog(); //in your case : OpenFileDialog

	private void OnGUI(){
		statsWindow = GUI.Window(0, statsWindow, statsGUIWindow, "Save Statistics");
		loadMovementWindow = GUI.Window(1, loadMovementWindow, loadMovementGUIWindow, "Load Movement");     
	}

	private void statsGUIWindow(int windowID){
		GUI.TextField(new Rect(margin, margin, statsWindow.width - margin * 2, 20), "Statistics Filename");
		if(GUI.Button(new Rect(statsWindow.width / 2 - 25, margin + 20, 50, 20), "Save")){
			System.Windows.Forms.SaveFileDialog saveStatisticsDialog = new System.Windows.Forms.SaveFileDialog();
			if (saveStatisticsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK){
				//Logic
			}
		}
		GUI.DragWindow();
	}

	private void loadMovementGUIWindow(int windowID){
		
		if (GUI.Button(new Rect(loadMovementWindow.width / 2 - 25, margin + 20, 50, 20), "Load")){
		System.Windows.Forms.OpenFileDialog openMovementDialog = new System.Windows.Forms.OpenFileDialog();
			if (openMovementDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK){
				fileHolder.movementFilename = openMovementDialog.FileName;
			}
		}
		GUI.DragWindow();

	}
}
