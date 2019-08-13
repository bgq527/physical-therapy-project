using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor_movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, jsontrainer.lowest_foot, gameObject.transform.position.z);
    }
}
