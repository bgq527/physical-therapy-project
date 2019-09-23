using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeConfidence : MonoBehaviour
{
    public GameObject Graph;
    public LineRenderer line;
    private Vector3[] points;
    public int pointsToDraw = 240;

    void Start()
    {
       
        //line.material = new Material(Shader.Find("Sprites/Default"));
        line.widthMultiplier = 0.0005f;
        // appx. last two seconds will be shown
        line.positionCount = pointsToDraw;
        points = new Vector3[9999];
        for (int i = 0; i < pointsToDraw; i++)
        {
            points[i] = new Vector3(i, 0, 2);
        }

        line.transform.position = new Vector3(0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Graph.transform.position = new Vector3(.552f, -.575f, 1.342f);
        //    Graph.transform.position = new Vector3(0, 1, -1);
        for (int i = 240-2; i > 1; i--)
        {
         //   points[i] = new Vector3(pointsToDraw - i, UnityEngine.Random.value, 2 );
            points[i + 1] = new Vector3(i, /*points[i].y*/ UnityEngine.Random.value*1, 0);
        }
        line.SetColors(Color.red, Color.green);
        points[0] = new Vector3(0, variable_holder.conf, 0);
        line.SetPositions(points);
    }
}
