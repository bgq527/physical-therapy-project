using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ProcessFlanker : MonoBehaviour
{
    public void ProcessFlankerData()
    {
        var psi = new ProcessStartInfo();
        psi.FileName = @"C:\Users\NIW\AppData\Local\Programs\Python\Python37-32\python.exe";

        var script = @"Desktop\newflankerprocessing.py";

        //psi.CreateNoWindow = true;

      //  psi.RedirectStandardError = true;

       // var errors = "";
        using (var process = Process.Start(psi))
        {
        //    errors = process.StandardError.ReadToEnd();

        }

       // print(errors);

    }
}