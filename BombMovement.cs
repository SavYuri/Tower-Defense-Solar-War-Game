using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{

    private Transform target;
    private Transform targetEnemy;
    public int bombSpeed;
    private int wavepointIndex;
    

    void Start()
    {
        target = Waypoints.points[Waypoints.points.Length - 1];
        wavepointIndex = Waypoints.points.Length;
    }

   
    void Update()
    {
        

           Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * bombSpeed * Time.deltaTime, Space.World);

        //враг смотрит в сторону направления движения
        transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, target.position) <= 1f)
        {
                GetNextWaypoint();
        }
    }


    public void GetNextWaypoint()
    {
        if (wavepointIndex == 0)
        {
            Destroy(gameObject);

            return;
        }

        wavepointIndex--;
        target = Waypoints.points[wavepointIndex];
    }

    

}
