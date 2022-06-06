using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationBomb : MonoBehaviour
{


    private Transform target;

    public int damage = 50;

    //радиус поражения должен быть больше 0
    public float explosionRadios;

    public GameObject impactEffect;

    public Turret turret;

    bool searchTarget;

    [HideInInspector] public Node installedNode;

    public bool samplePrifab;

    AudioSource audioS;

    public AudioClip roadBombDetonate;
   

    bool bombActivated;

    public GameObject ringEffect;

    public bool roadBomb;

    TurretStatistic turretStatistic;

    private void Start()
    {
        audioS = gameObject.GetComponent<AudioSource>();
        if (roadBomb)
        {
            ringEffect.SetActive(false);
        }
       
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, Quaternion.identity);

        
        Destroy(effectIns, 5f);

        if (explosionRadios > 0f)
        {
            Explode();
        }
        if (installedNode == null)
        {
            
        }
        else
        {
            installedNode.bombInstaled = false;
        }
        
        Destroy(gameObject);
        
    }

    //зона поражения от снаряда
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadios);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

   
    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }

    void roadBomdActivator()
    {
        if (bombActivated) return;

        bombActivated = true;
        ringEffect.SetActive(true);
        audioS.clip = roadBombDetonate;
        audioS.Play();
        Invoke("HitTarget", 1.5f);
        return;
    }

     private void OnTriggerEnter(Collider other)
     {
        if (other.tag == "Enemy")
        {
            if (samplePrifab) return;

            if (roadBomb)
            {
                roadBomdActivator();
            }
            else
            {
               
                HitTarget();
            }
           
               
        }
     }

   public void SetRoadBombStatistic(int index)
    {


        turretStatistic = TurretStatistic.turretStatisticClass;

        turretStatistic.HeadTurretName.text = "Road BOMB Level " + (index+1);
        turretStatistic.rangeTurret.text = "Explode: " + explosionRadios;
        turretStatistic.damageTurret.text = "Damage: " + damage;
        turretStatistic.fireRateTurret.text = "";
        turretStatistic.healthTurret.text = "";
   }

}
