using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointEnemyLand : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
            if (other.tag == "Enemy")
            {
            if (other.GetComponent<EnemyMovement>() == null) return;

            EnemyMovement EM = other.GetComponent<EnemyMovement>();

            
                if (!EM.enebleBackTime)
                {
                EM.GetNextWaypoint();
                }
                else
                {
                EM.GetBackWaypoint();
                }
            
        }
        
        


    }
}
