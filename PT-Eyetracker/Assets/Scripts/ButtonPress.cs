using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPress : MonoBehaviour
{
    

    public void changeFileName(string name)
    {
        fileHolder.saveFilename = name;
    }

    public void printConfirmation()
    {
        print(fileHolder.saveFilename);
    }
}
