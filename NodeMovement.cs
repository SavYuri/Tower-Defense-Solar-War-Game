using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeMovement : MonoBehaviour
{
    public Transform target;
    public int wavepointIndex = 0;
    public int speed;

    public Transform[] wayPoints;
    public WayPointsNodes wayPointsNodes;
    Vector3 dir;


    void Start()
    {
        
            wayPoints = wayPointsNodes.points;
        
       
        target = wayPoints[wavepointIndex];
    }

    void Update()
    {
       
        

        if (target == null)
        {
            target = wayPoints[wavepointIndex];
        }
        dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
           if (Vector3.Distance(transform.position, target.position) <= 4f)
           {
                GetNextWaypoint();  
           }

    }

    public void GetNextWaypoint()
    {
        if (wavepointIndex >= wayPoints.Length - 1)
        {

            wavepointIndex =0;
            target = wayPoints[wavepointIndex];
            return;
        }
        
        wavepointIndex++;
        target = wayPoints[wavepointIndex];
    }
    

   
}
