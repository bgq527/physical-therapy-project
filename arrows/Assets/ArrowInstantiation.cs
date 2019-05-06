using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInstantiation : MonoBehaviour {
    int arrowDirection;
    int frameCounter;
    int state;
    TextMesh arrowTextMesh;
    Transform cameraTransform;
    string arrows;
    Camera cameraCamera;

    // Use this for initialization
    void Start () {
        frameCounter = 0;
        arrowTextMesh = gameObject.GetComponent<TextMesh>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        cameraCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        arrows = createArrows();
        state = 1;
    }
	
	// Update is called once per frame
	void Update () {
        float cameraX = cameraTransform.transform.rotation.x;
        float cameraY = cameraTransform.transform.rotation.y;

        //arrowTextMesh.text = Mathf.Round(cameraX * 1000f) / 1000f + ", " + Mathf.Round(cameraY * 1000f) / 1000f;
        if (state == 0)
        {
            if (cameraX < -.15f && cameraY > .3f)
            // right
            {
                if (arrowTextMesh.text[2] == '>')
                {
                    cameraCamera.backgroundColor = Color.green;
                }
                else cameraCamera.backgroundColor = Color.red;
                state = 1;
                arrowTextMesh.text = "Look here";
                arrows = createArrows();
            }
            else if (cameraX < -.15f && cameraY < -.3f)
            // left
            {
                if (arrowTextMesh.text[2] == '<')
                {
                    cameraCamera.backgroundColor = Color.green;
                }
                else cameraCamera.backgroundColor = Color.red;
                state = 1;
                arrowTextMesh.text = "Look here";
                arrows = createArrows();
            }
        }

        if (state == 1)
        {
            if (cameraX > -.1f && cameraX < .1f && cameraY > -.1f && cameraY < .1f)
            {
                arrowTextMesh.text = arrows;
                cameraCamera.backgroundColor = Color.black;
                state = 0;
            }

        }
    }

    public string createArrows()
    {
        string arrowText = "";
        for (int i = 0; i < 5; i++)
        {
            if (Mathf.Round(Random.Range(0f, 1f)) == 1)
            {
                arrowText += "<";
            }
            else arrowText += ">";

        }
        return arrowText;
    }
}
