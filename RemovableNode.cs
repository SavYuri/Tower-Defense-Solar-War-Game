using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemovableNode : MonoBehaviour
{
    public Transform target;
    public int speed;
    Vector3 dir;
    Vector3 velocity = new Vector3(10, 0, 0);
   

    NodePoints nodePointsClass;

    private void Start()
    {
        nodePointsClass = NodePoints.nodePointsClass;
    }

    void Update()
    {
        GoToPosition();
    }

    void GoToPosition()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            target = null;
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, 1, speed, Time.deltaTime);
 
    }
}
