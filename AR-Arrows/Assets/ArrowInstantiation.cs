using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class ArrowInstantiation : MonoBehaviour {
    int arrowDirection;
    int frameCounter;
    int state;
    TextMesh arrowTextMesh;
    Transform cameraTransform;
    string arrows;
    Camera cameraCamera;
    Timer timer;

    // Use this for initialization
    void Start () {
        frameCounter = 0;
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<TextMesh>();
        cameraTransform = GameObject.Find("ARCamera").GetComponent<Transform>();
        cameraCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        arrows = createArrows();
        state = 1;
    }
	
	// Update is called once per frame
	void Update () {
        //arrowTextMesh.text = cameraTransform.transform.rotation.x + " | " + cameraTransform.transform.rotation.y;
        float cameraX = cameraTransform.transform.rotation.x;
        float cameraY = cameraTransform.transform.rotation.y;

        //arrowTextMesh.text = Mathf.Round(cameraX * 1000f) / 1000f + ", " + Mathf.Round(cameraY * 1000f) / 1000f;

        // The following checks which state the program is in
        // state 0 is the time when the test is currently active
        // it starts when the user looks at the center and ends when the user looks at a target

        // state 1 is the time in between tests when the user is recentering their head
        // it begins when the user looks at the target and ends when the user looks at the "Look here" target 

        frameCounter++;
        if (frameCounter % 100 == 0) print(createArrows());

        if (state == 0)
        {
            // Checks if the user hit either target
            if ((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f))
            {
                if (checkCorrect(cameraY))
                {
                    cameraCamera.backgroundColor = Color.green;
                }
                else
                {
                    cameraCamera.backgroundColor = Color.red;
                }

                state = 1;
                arrowTextMesh.text = "+";
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

    // This method checks if the user looked at the correct target
    public bool checkCorrect(float y)
    {
        bool correct;
        if (y > .3f) // checks if the user was looking at the right
        {
            correct = arrowTextMesh.text[2] == '>' ? true : false;
        }
        else // check if the user was looking at the left
        {
            correct = arrowTextMesh.text[2] == '<' ? true : false;
        }
        return correct;
    } // checkCorrect(float)

    // This method creates a String of five arrows pointing in random directions
    public string createArrows()
    {
        string[] arrowText = {"<<<<",">>>>>","<<><<",">><>>"};
        
        return arrowText[(int)(Random.Range(0f, 3f))];
    } // createArrows()
}
