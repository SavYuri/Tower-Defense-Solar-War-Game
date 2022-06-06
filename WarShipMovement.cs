using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarShipMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    public bool enebleBackTime;

    

    public int speed;

    

    void Start()
    {

      


        target = WayWarShipPoints.points[0];

    }

    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        //враг смотрит в сторону направления движения
        //transform.rotation = Quaternion.LookRotation(dir);
        LockOnTarget();

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            
                GetNextWaypoint();
           
        }

        

    }

    //плавный поворот в сторону точки цели
    void LockOnTarget()
    {

        //захват цели
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 2).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);


    }
    public void GetNextWaypoint()
    {
        if (wavepointIndex >= WayWarShipPoints.points.Length - 1)
        {
            EndPath();

            return;
        }

        wavepointIndex++;
        target = WayWarShipPoints.points[wavepointIndex];
    }

   

    void EndPath()
    {
       
        Destroy(gameObject);
    }


   
}
