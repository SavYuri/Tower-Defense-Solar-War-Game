using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public Transform target;
    public Enemy targetEnemy;

    [Header("General")]
    
    public float range;
    public GameObject GizmoRange;
    [Header("Use Bullets (default)")]
    public GameObject buletPrefab;
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    //при изменении переменной нужно добавить точки для ракет в скрипте NuclearMissleSystem
    public int countOfMissiles;



    AudioSource audioSource;
    public AudioClip turretShot;

    public bool useNuclearMissile = false;
    public NuclearMissleSystem nuclearMissleSystem;
    [Header("Use Laser")]
    public bool useLaser = false;
    

    public int damageOverTime = 30;
    //замедление
    public float slowAmount = .5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    public AudioClip laserSound;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;

    //скорость вращения башни
    public float turnSpeed = 10f;

   
    public Transform firePoint;

    public static Turret turretClass;

    public GameObject ShotEffect;

    public TurretStatistic turretStatistic;

    public GameObject buildTimeBar;

    public Node nodeToBuild;

    float buildTime;
    float buildTimer;

    public TurretBlueprint buildTurret;

    public bool buildInProcess;

    //public bool nuclearMissle;
    public GameObject turretObject;
    public bool EnemyDroneLaser;
    public bool energyTurret;
    public bool airStriker;
    public bool enemyTurret;
    public bool turretIsFrozen;
    public bool frozenEnemyLockOnTurret;

 

    private void Awake()
    {
        if (energyTurret)
        {
            EnergyGeneratorAwake();
        }

        if (useLaser)
        {
            LaserAwake();
        }

        audioSource = GetComponent<AudioSource>();

        buildInProcess = false;


        if (turretClass != null)
        {

            return;
        }
        else
        {
            turretClass = this;
        }

        
       
       
    }

    void EnergyGeneratorAwake()
    {
        
        buletPrefab = null;
       
    }
    void LaserAwake()
    {
        
        buletPrefab = null;
        ShotEffect = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (buildTimeBar != null)
        {
            buildTimeBar.SetActive(false);
        }


        
        if (EnemyDroneLaser || enemyTurret)
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }
        else if (airStriker)
        {
            InvokeRepeating("UpdateAirTarget", 0f, 0.5f);
            
        }
        else
        {
            InvokeRepeating("FindNearestTargetToEnd", 0f, 0.5f);
        }

       

    }

    //ближайшая цель к финишу
    void FindNearestTargetToEnd()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestToEnd = null;
        int biggestIndextoEnd = 0;
        float nearestToPoint = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= range)
            {
                if(enemy.GetComponent<EnemyMovement>() != null)
                {
                    if (enemy.GetComponent<EnemyMovement>().wavepointIndex >= biggestIndextoEnd)
                    {
                        biggestIndextoEnd = enemy.GetComponent<EnemyMovement>().wavepointIndex;
                        float distanceToPoint = Vector3.Distance(enemy.transform.position, Waypoints.points[enemy.GetComponent<EnemyMovement>().wavepointIndex].transform.position);

                        if (nearestToPoint >= distanceToPoint)
                        {
                            nearestToPoint = distanceToPoint;
                            nearestToEnd = enemy;
                        }
                        
                       
                    }
                }
               

            }
        }

        if (nearestToEnd != null)
        {
            target = nearestToEnd.transform;
            targetEnemy = nearestToEnd.GetComponent<Enemy>();
           
        }
        else
        {
            target = null;
        }
    }

    //Ближайшая цель к пушке
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
 
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
               
            }

            
        }

        
       

       if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
           
        }
        else
        {
            target = null;
            targetEnemy = null;
        }
    }

    //Ближайшая цель к ПВО с приоритетом на самолёты 
    void UpdateAirTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies == null) return;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject nearestFlyEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {

                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null)
            {
                if (nearestEnemy.GetComponent<Enemy>().enemyTargetPriority == 1)
                {
                    nearestFlyEnemy = nearestEnemy;
                }
            }
            


        }

        if (nearestFlyEnemy != null && shortestDistance <= range)
        {
            target = nearestFlyEnemy.transform;
            targetEnemy = nearestFlyEnemy.GetComponent<Enemy>();
            return;
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        

        if (turretStatistic != null)
        {
            turretStatistic.healthTurret.text = "Health: " + gameObject.GetComponent<Enemy>().health.ToString() + "/" + gameObject.GetComponent<Enemy>().startHealth.ToString();

        }

        BuildTimer();
        fireCountdown -= Time.deltaTime;
        


        if (target == null || turretIsFrozen)
        {
            //убирает луч лазера если цель вне зоны поражения
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    //стоп эффект от лазера
                    if (audioSource != null)
                    {
                        audioSource.Stop();
                    }
                    
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }
        if (turretIsFrozen) return;


        LockOnTarget();
       

        if (useLaser)
        {
            Laser();
        }
        
        else
        {
            if (fireCountdown <= 0f)
            {
                if (useNuclearMissile)
                {
                    ShootNuclearMissile();
                }
                else
                {
                    
                    
                    Shoot();
                }
                
                fireCountdown = 1f / fireRate;
            }

            
            
        }

        
    }

    


    public void BuildTurret()
    {
        StartCoroutine("ToBuild");
    }
    public void UpgradeTurret()
    {
        StartCoroutine("ToUpgrade");
    }


    IEnumerator ToBuild()
    {
        PlayerStats.Money -= nodeToBuild.turretBlueprint.cost;

        if (gameObject.GetComponent<Enemy>().thisIsEnergyGenerator)
        {
            Shop.ShopClass.energyGeneratorAvailableCount--;
        }

        buildInProcess = true;
        buildTimeBar.SetActive(true);
        buildTime = nodeToBuild.turretBlueprint.timeBuildPrefab;
        buildTimer = buildTime;
        nodeToBuild.ActivateGizmo(false);
        Shop.ShopClass.DisableTurretsImages();
        
        //Задержка при строительстве
        yield return new WaitForSeconds(nodeToBuild.turretBlueprint.timeBuildPrefab);
        buildTimeBar.SetActive(false);
        //Shop.ShopClass.EnableShopButtons();
        buildInProcess = false;
        nodeToBuild.BuildTurretButton();
        
        // Shop.ShopClass.EnableShopButtons();
        yield break;
    }

    IEnumerator ToUpgrade()
    {
        
        float _time = 0;
        if (nodeToBuild.isUpgraded == false)
        {
            _time = nodeToBuild.turretBlueprint.timeUpgradePrefab;
            PlayerStats.Money -= nodeToBuild.turretBlueprint.upgradeCost;
        }
        else if (nodeToBuild.isUpgraded2 == false)
        {
            _time = nodeToBuild.turretBlueprint.timeUpgradePrefab2;
            PlayerStats.Money -= nodeToBuild.turretBlueprint.upgradeCost2;
        }
        else if (nodeToBuild.isUpgraded3 == false)
        {
            _time = nodeToBuild.turretBlueprint.timeUpgradePrefab3;
            PlayerStats.Money -= nodeToBuild.turretBlueprint.upgradeCost3;
        }
            buildInProcess = true;
        //неактивность турели
            enemyTag = "nobody";
            NodeUI.nodeUI.ui.SetActive(false);
            buildTimeBar.SetActive(true);
            buildTime = _time;
            buildTimer = buildTime;
            nodeToBuild.ActivateGizmo(false);
            Shop.ShopClass.DisableTurretsImages();
            Shop.ShopClass.EnableShopButtons();
            //Задержка при строительстве
            yield return new WaitForSeconds(buildTime);
            buildTimeBar.SetActive(false);
            enemyTag = "Enemy";
            buildInProcess = false;
            nodeToBuild.UpgradeTurret();
            BuildManager.instance.selectedNode = null;
            
        yield break;
        
    }

    void BuildTimer()
    {
        if (buildTime <= 0)
        {
            return;
        }
        else
        {
            buildTimer -= Time.deltaTime;
            buildTimeBar.GetComponent<Image>().fillAmount = 1f - (buildTimer / buildTime);

        }
    }

    public void SetTurretStatistic()
    {
        turretStatistic = TurretStatistic.turretStatisticClass;
        turretStatistic.rangeTurret.text = "Range: " + range.ToString();
        if (useNuclearMissile)
        {
            turretStatistic.damageTurret.text = "Damage: " + buletPrefab.GetComponent<NuclearBullet>().damage.ToString();
        }
        else if (energyTurret)
        {
            turretStatistic.damageTurret.text = "Energy: " + gameObject.GetComponent<EnergyGenerator>().energyReward.ToString();
        }
        else if (useLaser)
        {
            turretStatistic.damageTurret.text = "Damage: " + damageOverTime.ToString();
        }
        else
        {
            turretStatistic.damageTurret.text = "Damage: " + buletPrefab.GetComponent<Bullet>().damage.ToString();
        }

        if (energyTurret)
        {
            turretStatistic.fireRateTurret.text = "Time: " + gameObject.GetComponent<EnergyGenerator>().cycleTime.ToString();
        }
        else if (useLaser)
        {
            turretStatistic.fireRateTurret.text = "Slow Effect: " + (slowAmount*100).ToString() + "%";
        }
        else
        turretStatistic.fireRateTurret.text = "Fire Rate: " + fireRate.ToString();

        turretStatistic.healthTurret.text = "Health: " + gameObject.GetComponent<Enemy>().health.ToString() + "/" + gameObject.GetComponent<Enemy>().startHealth.ToString();

    }

   

    void LockOnTarget()
    {
      
            //захват цели
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
       

    }

    void LockOnTarget2()
    {
      
    }


    void Laser()
    {
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        }
        else return;
        
        if (!EnemyDroneLaser)
        {
            targetEnemy.Slow(slowAmount);
        }
        
        

        //рисует луч лазера
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.clip = laserSound;
                audioSource.Play();
            }
            
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        
        Vector3 dir = firePoint.position - target.position;
        //присваиваем позицию эффекта лазера к позиции цели
        impactEffect.transform.position = target.position + dir.normalized;
        //направляет искры эффекта в сторону луча
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

    }

    void ShootNuclearMissile()
    {
        nuclearMissleSystem.StartCoroutine("MissileLauncher");
    }

    void Shoot()
    {
        if (target != null && target.GetComponent<Turret>() != null)
        {
            if (target.GetComponent<Turret>().buildInProcess) return;
        }
        audioSource.clip = turretShot;
        audioSource.Play();
        
        
            GameObject ShotFire = (GameObject)Instantiate(ShotEffect, firePoint.position, firePoint.rotation);
            Destroy(ShotFire, 1f);
            GameObject bulletGo = (GameObject)Instantiate(buletPrefab, firePoint.position, firePoint.rotation);
        
            Bullet bullet = bulletGo.GetComponent<Bullet>();
        
            if (bullet != null)
            {
            bullet.Seek(target);
            bullet.turret = this;
            }
                
        
    }

    //рисует область поражения турели
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
