using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorScript : MonoBehaviour
{

    Projector proj;
    GameObject body;

    // Start is called before the first frame update
    void Start()
    {
        proj = GetComponent<Projector>();
        body = GameObject.Find("rp_eric_rigged_001_yup_t");
    }

    // Update is called once per frame
    void Update()
    {
        var position = body.transform.position;
        position.z = position.z - 25;
        position.y = 36.2f;
        proj.transform.position = position;
    }
}
