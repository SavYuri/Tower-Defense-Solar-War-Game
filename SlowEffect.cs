using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public TurretSlow turretSlow;
    public int levelOfSlowTurret;

    void Start()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().ActivityTurret(levelOfSlowTurret, true);
            other.GetComponent<Enemy>().Slow(turretSlow.slowAmount);
            
        }
        else return;
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().ActivityTurret(levelOfSlowTurret, false);
            other.GetComponent<Enemy>().speed = other.GetComponent<Enemy>().startSpeed;
           
        }

        else return;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().ActivityTurret(levelOfSlowTurret, true);
            other.GetComponent<Enemy>().Slow(turretSlow.slowAmount);
            
        }
        else return;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretSlow.range);
    }

}
