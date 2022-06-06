using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairDronesPoints : MonoBehaviour
{
    [HideInInspector]
    public static Transform[] waitDronPoints;
    [HideInInspector]
    
    public static RepairDronesPoints repairDronesPoints;

    public Transform pointGoAway;

   
    void Start()
    {
        if (repairDronesPoints != null) return; else repairDronesPoints = this;

        InitialWaitDronesPoint();

        Debug.Log("DronPoints:" + waitDronPoints.Length);
        

    }

    void InitialWaitDronesPoint()
    {
        waitDronPoints = new Transform[gameObject.transform.childCount];
        for (int i = 0; i < waitDronPoints.Length; i++)
        {
            waitDronPoints[i] = gameObject.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform FreeWaitDronPoint()
    {

        for (int i = 0; i < waitDronPoints.Length; i++)
        {
            bool pointIsBusy = waitDronPoints[i].gameObject.GetComponent<RepairDronePoint>().pointIsBusy;

            if (!pointIsBusy)
            {
                waitDronPoints[i].gameObject.GetComponent<RepairDronePoint>().pointIsBusy = true;
                return waitDronPoints[i];
 
            }
          
        }
        return null;

    }
}
