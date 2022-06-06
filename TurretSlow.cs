using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlow : MonoBehaviour
{
    private Transform [] target;
    public Enemy[] targetEnemy;
    [Header("General")]
    public float range;
    public GameObject purticalEffect;
    public GameObject GizmoRange;
    [Header("Use Bullets (default)")]
    AudioSource audioSource;
    public AudioClip turretShot;
    [Header("Use Laser")]
    public float slowAmount = .5f;
    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public static TurretSlow turretClass;
    public GameObject ShotEffect;
    public int levelOfTurret;
    public Node nodeToBuild;

    public GameObject buildTimeBar;
    public CapsuleCollider slowEffectZone;

    float buildTime;
    float buildTimer;

    public TurretBlueprint buildTurret;

    public bool buildInProcess;

    public TurretStatistic turretStatistic;
    private void Awake()
    {

        turretStatistic = TurretStatistic.turretStatisticClass;

        if (turretClass != null)
        {

            return;
        }
        turretClass = this;


        //размер эффекта анимации замедления
        var main = purticalEffect.GetComponentInParent<ParticleSystem>().main;
        main.startSize = range;
        slowEffectZone.radius = range;
    }

    
    void Start()
    {
        turretStatistic = TurretStatistic.turretStatisticClass;
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);

        if (buildTimeBar != null)
        {
            buildTimeBar.SetActive(false);
        }


        //размер эффекта анимации замедления
        var main = purticalEffect.GetComponentInParent<ParticleSystem>().main;
        main.startSize = range;
        slowEffectZone.radius = range;

    }

    
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        targetEnemy = new Enemy[enemies.Length];

            for (int i = 0; i < enemies.Length; i++) {
                float  distanceToEnemy = Vector3.Distance(transform.position, enemies[i].transform.position);
                if (distanceToEnemy <= range)
                {
                targetEnemy[i] = enemies[i].GetComponent<Enemy>();
               // targetEnemy[i].LevelOfNewTurret(levelOfTurret);
                targetEnemy[i].Slow(slowAmount);
            }
                else
                {
                
                targetEnemy[i] = null;
                }
            }
    }

    

    void Update()
    {
        

        BuildTimer();
    }

    //рисует область поражения турели
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    

    public void SetTurretStatistic()
    {
        turretStatistic.HeadTurretName.text = "Slow Effect Turret";
        turretStatistic.rangeTurret.text = "Range: " + range.ToString();
        turretStatistic.damageTurret.text = "Damage: - " ;
        turretStatistic.fireRateTurret.text = "Fire Rate: - ";


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

}

   


