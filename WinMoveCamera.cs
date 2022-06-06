using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class WinMoveCamera : MonoBehaviour
{

    public Transform target;
    public static WinMoveCamera winMoveCamera;

    public bool moveEnable;
    float x;
    float y;
    float z;

    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (winMoveCamera != null) return;
        else winMoveCamera = this;


        moveEnable = false;
        
    }

    Vector3 velocity = new Vector3(0,15,0);

    // Update is called once per frame
    void Update()
    {
        if (moveEnable)
        {

           // Vector3 dir = target.position - transform.position;
           // transform.Translate(dir.normalized * 150 * Time.deltaTime, Space.World);

            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, 1, 150, Time.deltaTime);
            transform.rotation = target.rotation;
            // transform.rotation = Quaternion.LookRotation(dir);
        }

        if (Vector3.Distance(transform.position, target.position) <= 5f)
        {
            moveEnable = false;
        }
    }

    public void SwitchCameraMovement()
    {
        moveEnable = true;
    }

    
}
