using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBScript : MonoBehaviour
{
    GameObject camera;
    Color lerpedColor = Color.blue;
    float time;
    Color[] cycle;
    int count;
    Camera cam;
    float timeBetweenCycles;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        time = 0f;
        cycle = new Color[12] { new Color(.32f,.0549f,.4039f, 1), Color.black, Color.blue, Color.black, Color.green, Color.black, Color.yellow, Color.black, new Color(1, 0.45490f, 0.1803f, 1), Color.black, Color.red, Color.black };
        count = 0;
        cam = camera.GetComponent<Camera>();
        timeBetweenCycles = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        print(time);

        if (time > timeBetweenCycles && count <= 12)
        {
            cam.backgroundColor = cycle[count];
            count++;
            time -= timeBetweenCycles;
        }
       
    
    }
}
