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
    string[] arrowText;
    int currentArrow;
    int startFrame;
   // Timer timer;

    // Use this for initialization
    void Start () {
        frameCounter = 0;
        arrowTextMesh = GameObject.Find("arrowText").GetComponent<TextMesh>();
        cameraTransform = GameObject.Find("ARCamera").GetComponent<Transform>();
        cameraCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        arrowText = new string[4];
        arrowText[0] = "<<<<<";
        arrowText[1] = ">>>>>";
        arrowText[2] = "<<><<";
        arrowText[3] = ">><>>";
        arrows = arrowText[createArrows()];
        state = 1;
        //timer = new Timer(1000);


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
        // 60 fps * .25 seconds = 15 frames 

        if (state == 0)
        {

            // Makes the command disappear after ~250 ms
            if (frameCounter > 15)
            {
                arrowTextMesh.text = "+";
            }
            // Checks if the user hit either target
            if ((cameraX < -.15f) && (cameraY < -.3f || cameraY > .3f))
            {
                if (checkCorrect(cameraY))
                {
                    // cameraCamera.backgroundColor = Color.green;
                    print("Correct");
                }
                else
                {
                    // cameraCamera.backgroundColor = Color.red;
                    print("Incorrect");
                }

                state = 1;
                arrowTextMesh.text = "+";
                currentArrow = createArrows();
                arrows = arrowText[currentArrow];

            }
            // if 1 second has passed start new command to keep pace
            else if (frameCounter > 60)
            {
                state = 1;
                arrowTextMesh.text = "+";
                currentArrow = createArrows();
                arrows = arrowText[currentArrow];
            }
        }

        if (state == 1)
        {
            if (cameraX > -.1f && cameraX < .1f && cameraY > -.1f && cameraY < .1f)
            {
                frameCounter = 0;
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
            correct = (currentArrow==0||currentArrow==3) ? true : false;
        }
        else // check if the user was looking at the left
        {
            correct = (currentArrow == 1 || currentArrow == 2) ? true : false;
        }

        return correct;
    } // checkCorrect(float)

    // This method creates a String of five arrows pointing in random directions
    public int createArrows()
    {
        return (int) (Random.Range(0f, 3.99f));
    } // createArrows()
}
