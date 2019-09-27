using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class ProcessFlanker : MonoBehaviour
{
    public void ProcessFlankerData()
    {
        foreach (string file in Directory.GetFiles(@"C:\Users\NIW\Desktop\flankerRaw", "*.csv")) 
        {
            File.AppendAllText(@"C:\Users\NIW\Desktop\combined_csv.csv", File.ReadAllText(file));

            File.Delete(file);
        }

       
    }
}