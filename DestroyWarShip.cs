using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWarShip : MonoBehaviour
{
    Vector3 curPos;
    Vector3 lastPos;
    PauseMenu PM;

    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("GameMaster").GetComponent<PauseMenu>();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        curPos = transform.position;
        if (curPos == lastPos && PM.ui.activeSelf == false)
        {  
                Destroy(gameObject);
        }
        lastPos = curPos;
    }
    */
    
}
