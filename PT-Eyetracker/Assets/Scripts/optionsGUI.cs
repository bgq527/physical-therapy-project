using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class optionsGUI : MonoBehaviour
{
    public int margin = 15;
    private float threshold = 0.1f;
    private float scale = 0.15f;
    //private Rect statsWindow = new Rect(Screen.width - 265, 15, 250, 70);
    private Rect loadMovementWindow = new Rect(Screen.width - 265, 15, 250, 70);
    //private Rect movementOptionsWindow = new Rect(Screen.width - 265, 155, 250, 140);
    string comparisonThreshold;
    string boxScale;
    string saveFileName;

    [DllImport("user32.dll")]
    private static extern void OpenFileDialog(); //in your case : OpenFileDialog

    private void OnGUI()
    {
        //statsWindow = GUI.Window(0, statsWindow, statsGUIWindow, "Save Statistics");
        loadMovementWindow = GUI.Window(1, loadMovementWindow, loadMovementGUIWindow, "Load Movement");
    //    movementOptionsWindow = GUI.Window(2, movementOptionsWindow, movementOptionsGUIWindow, "Options");
    }

    //private void statsGUIWindow(int windowID)
    //{
    //    saveFileName = GUI.TextField(new Rect(margin, margin, statsWindow.width - margin * 2, 20), saveFileName);
    //    if (GUI.Button(new Rect(statsWindow.width / 2 - 25, margin + 20, 50, 20), "Save"))
    //    {
    //        System.Windows.Forms.SaveFileDialog saveStatisticsDialog = new System.Windows.Forms.SaveFileDialog();
    //        if (saveStatisticsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //        {
    //            fileHolder.saveFilename = saveFileName;
    //        }
    //    }
    //    GUI.DragWindow();
    //}

    private void loadMovementGUIWindow(int windowID)
    {

        if (GUI.Button(new Rect(loadMovementWindow.width / 2 - 25, margin + 20, 50, 20), "Load"))
        {
            System.Windows.Forms.OpenFileDialog openMovementDialog = new System.Windows.Forms.OpenFileDialog();
            openMovementDialog.InitialDirectory = "Assets";
            openMovementDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (openMovementDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileHolder.movementFilename = openMovementDialog.FileName;
            }
        }
        GUI.DragWindow();

    }

    //private void movementOptionsGUIWindow(int windowID)
    //{
    //    GUI.Label(new Rect(margin, margin, movementOptionsWindow.width - margin * 2, 20), "Set Threshold (default is .1)");
    //    comparisonThreshold = GUI.TextField(new Rect(margin, margin + 20, movementOptionsWindow.width - margin * 2, 20), comparisonThreshold);

    //    GUI.Label(new Rect(margin, margin * 2 + 25, movementOptionsWindow.width - margin * 2, 20), "Set box scale (default is .15)");
    //    boxScale = GUI.TextField(new Rect(margin, margin * 2 + 45, movementOptionsWindow.width - margin * 2, 20), boxScale);
    //    if (float.Parse(boxScale) != 0 || float.Parse(boxScale) != fileHolder.scale)
    //    {
    //        fileHolder.scale = float.Parse(boxScale);
    //    }

    //    if (float.Parse(comparisonThreshold) != 0 || float.Parse(comparisonThreshold) != fileHolder.threshold)
    //    {
    //        fileHolder.threshold = float.Parse(comparisonThreshold);
    //    }


    //    GUI.DragWindow();
    //}
}