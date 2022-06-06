using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class FlyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    public bool enebleBackTime;

    private Enemy enemy;

    

    void Start()
    {

        enemy = GetComponent<Enemy>();
        
        
            target = WayFlyPoints.points[0];
       
    }

    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        //враг смотрит в сторону направления движения
        enemy.transform.rotation = Quaternion.LookRotation(dir);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            if (!enebleBackTime)
            {
                GetNextWaypoint();
            }
            else
            {
                GetBackWaypoint();
            }
        }

        //возвращает исходную скорость врагу если луч лазера не активен
        enemy.speed = enemy.startSpeed;

    }
    public void GetNextWaypoint()
    {
        if (wavepointIndex >= WayFlyPoints.points.Length - 1)
        {
            EndPath();

            return;
        }

        wavepointIndex++;
        target = WayFlyPoints.points[wavepointIndex];
    }

    public void GetBackWaypoint()
    {
        if (wavepointIndex <= 0)
        {
            enemy.speed = 0;

            return;
        }

        wavepointIndex--;
        target = WayFlyPoints.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
