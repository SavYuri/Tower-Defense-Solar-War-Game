using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairDrone : MonoBehaviour
{
    public float speed = 20f;
    GameObject targetToRepair;
    RepairDronesPoints RDPoints;
    Transform waitPoint;
    Transform correctTurretToRapair;

    float startTotalRepair;
    public float totalRepair;
    public float healthRepair;

    bool repairInProgres;
    bool repairEmpty;
    public GameObject repairEffect;

    public Image repairLavelBar;

    void Start()
    {
        startTotalRepair = totalRepair;
        repairEffect.SetActive(false);
        RDPoints = RepairDronesPoints.repairDronesPoints;

        InvokeRepeating("FindTurretWithMaximalDamage", 0, 2f);
        
    }

    
    void Update()
    {
        repairLavelBar.fillAmount = totalRepair / startTotalRepair;
        Movement();
        RepairEffect();
    }

    void RepairEffect()
    {
        if (repairInProgres && correctTurretToRapair != null)
        {
            repairEffect.SetActive(true);
        }
        else
        {
            repairEffect.SetActive(false);
        }
    }

   void Movement()
   {
        if (repairEmpty)
        {
            GoToPosition(RDPoints.pointGoAway);
        }
        else if (correctTurretToRapair != null)
        {
            GoToPosition(correctTurretToRapair);
            
        }
        else if (targetToRepair != null)
        {
            GetTurretToRapair();
            
        }
        else if (waitPoint != null)
        {
            GoToPosition(waitPoint);
        }
        else
        {
            GetFreeWaitPoint();
        }
   }
   

    void GetFreeWaitPoint()
    {
        waitPoint = RDPoints.FreeWaitDronPoint();
        
    }

    void GetTurretToRapair()
    { 
        if (targetToRepair.GetComponent<Enemy>().enableRepairDroneActivity == true) return;
        correctTurretToRapair = targetToRepair.transform;
        targetToRepair.GetComponent<Enemy>().enableRepairDroneActivity = true;
        if (waitPoint != null)
        {
            waitPoint.gameObject.GetComponent<RepairDronePoint>().pointIsBusy = false;
            waitPoint = null;
        }

    }

    Vector3 velocity = new Vector3(10, 0, 0); 

    void GoToPosition(Transform target)
    {
        if (target == null)
        {
            waitPoint = null;
            correctTurretToRapair = null;
            return;
        }

        Vector3 dir = target.position - transform.position;
       
        if (Vector3.Distance(transform.position, target.position) <= 1f)
        {
            if (repairInProgres) return;

            if (target == correctTurretToRapair)
            {
                StartCoroutine("ToRepair");
            }

            return;
        }

        



        transform.position =  Vector3.SmoothDamp(transform.position, target.position, ref velocity, 1, speed, Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 10f)
        {
            if (repairInProgres) return;

            if (target == correctTurretToRapair)
            {
                StartCoroutine("ToRepair");
            }

            return;
        }

        // transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        //transform.rotation = Quaternion.LookRotation(dir);
    }

    void FindTurretWithMaximalDamage()
    {
     
        GameObject[] turrets;
        GameObject turretWithMaximalDamage = null;
        float minimalLevelOfHealth = 100; //%
        turrets = GameObject.FindGameObjectsWithTag("Turret");
       
        if (turrets.Length == 0)
        {
            return;
        }

        //foreach (GameObject turret in turrets)
        for (int i = 0; i < turrets.Length; i++)
        {
            if (turrets[i].GetComponent<Enemy>() == null) continue;
            //Enemy obj = turrets[i].GetComponent<Enemy>();
            if (turrets[i].GetComponent<Enemy>().enableRepairDroneActivity) continue;
            float turretStartHealth = turrets[i].GetComponent<Enemy>().startHealth;
            float turretHealth = turrets[i].GetComponent<Enemy>().health;
            if (turretHealth == turretStartHealth) continue;
            float turretLevelOfHealth = (turretHealth / turretStartHealth) * 100;

            if(turretLevelOfHealth < minimalLevelOfHealth)
            {
                minimalLevelOfHealth = turretLevelOfHealth;
                turretWithMaximalDamage = turrets[i];
                
            }
        }
       
        targetToRepair = turretWithMaximalDamage;
        Debug.Log(targetToRepair);
    }

    IEnumerator ToRepair()
    {
        float turretStartHealth = correctTurretToRapair.gameObject.GetComponent<Enemy>().startHealth;
        float turretHealth = correctTurretToRapair.gameObject.GetComponent<Enemy>().health;

        while (totalRepair >= healthRepair && turretHealth <= turretStartHealth)
        {
            repairInProgres = true;
            correctTurretToRapair.gameObject.GetComponent<Enemy>().health += healthRepair;
            totalRepair -= healthRepair;
            yield return new WaitForSeconds(1);
            turretHealth = correctTurretToRapair.gameObject.GetComponent<Enemy>().health;

        }

        if (turretHealth >= turretStartHealth)
        {
            correctTurretToRapair.gameObject.GetComponent<Enemy>().health = correctTurretToRapair.GetComponent<Enemy>().startHealth;
            correctTurretToRapair.gameObject.GetComponent<Enemy>().enableRepairDroneActivity = false;
            correctTurretToRapair = null;
            repairInProgres = false;
        }

        if (totalRepair < healthRepair)
        {
            repairEmpty = true;
            if (correctTurretToRapair != null)
            {
                correctTurretToRapair.gameObject.GetComponent<Enemy>().enableRepairDroneActivity = false;
            }
            
            correctTurretToRapair = null;
            repairInProgres = false;
        }
    }

   
}
