using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    
    public Transform MainCamera;
        void Update()
        {
        // Point the object at the world origin (0,0,0)
        transform.rotation = Quaternion.Euler(40, 0, 0);
    }
    
}
