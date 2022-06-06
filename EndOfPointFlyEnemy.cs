using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPointFlyEnemy : MonoBehaviour
{

    public bool enemyGoAway;
    public bool destroyCheckPoint;
   

    private void OnTriggerEnter(Collider other)
    {
        if (enemyGoAway)
        {
            if (other.tag == "FlyEnemy")
            {
                other.GetComponent<Enemy>().FlyEnemySurviwe();
            }
        }
        if (destroyCheckPoint)
        {
            Destroy(other.gameObject);
        }
       

    }
}
