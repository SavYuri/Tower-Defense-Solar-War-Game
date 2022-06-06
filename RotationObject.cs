using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    

    public int speed = 5;
    public bool flyWay;

    public float x;
    public float y;
    public float z;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        

        if (flyWay)
        {
            transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
        
    }
}
