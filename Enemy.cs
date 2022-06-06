using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector] //убирает из панели инспектора юнити
    public float speed;

    public float startHealth = 100;
    public float health;

    //бонус за уничтожение врага
    public int worth = 50;
    public int worthEnergy = 10;

    //эффект при уничтожении врага
    public GameObject deathEffect;

    //полоска жизни
    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;

    AudioSource audioSource;
    public AudioClip destroySound;
    TurretSlow turretSlow;
    public static Enemy enemyClass;
    //private int levelOfNewSlowTurret;
    //private int levelOfCorrectSlowTurret = 0;
    bool[] levelOfSlowTurretActivity;

    public GameObject[] enemyDrones;

    public bool thisIsMissile;

    // 0 - standart; 1 - airShips; 2 - enemyFlyDrones
    public int enemyTargetPriority;

    public GameObject healthSmoke;

    public bool thisScriptOnTurret;

    public bool thisIsEnergyGenerator;

    public bool repairHealthInProcess;

    public bool enableRepairDroneActivity;

    

    private void Awake()
    {
      
       
        if (enemyClass != null) return;
        else
        {
            enemyClass = this;
        }

        
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (healthSmoke != null)
        {
            healthSmoke.SetActive(false);
        }
        
        InitializeLevelOfSlowTurretActivity();
        if (!thisIsMissile && !thisScriptOnTurret)
        {
            WaveSpawner.EnemiesAlive++;
           
        }
        
        Debug.Log("Враги: " + WaveSpawner.EnemiesAlive);
        speed = startSpeed;
        SetDifficultLevel();
    }
    private void Update()
    {
        HealthBarActivator();
        if (thisScriptOnTurret)
        {
            damageHealthSmoke();
        }
       
    }

    void SetDifficultLevel()
    {
        int difficultyLevel = PlayerPrefs.GetInt("LevelOfDifficulty");

        if (difficultyLevel == 0)
        {
            if (!thisScriptOnTurret)
            {
                health = startHealth * 0.8f;
            }
            else
            {
                health = startHealth;
            }
           
            worth += 2;
        }
        else if (difficultyLevel == 1)
        {

            health = startHealth;
            worth += 3;
        }
        else if (difficultyLevel == 2)
        {

            if (!thisScriptOnTurret)
            {
                health = startHealth * 1.2f;
            }
            else
            {
                health = startHealth;
            }

            
            worth += 4;
            
        }
    }

    void damageHealthSmoke()
    {
        
        if (health < startHealth / 2)
        {
            healthSmoke.SetActive(true);
        }
        else 
        {
            healthSmoke.SetActive(false);
        }
    }

    void HealthBarActivator()
    {
        if (health != startHealth)
        {
            healthBar.color = new Color(healthBar.color.r, healthBar.color.g, healthBar.color.b, 255f);
        }
        else
        {
            healthBar.color = new Color(healthBar.color.r, healthBar.color.g, healthBar.color.b, 0f);
        }
    }

    void InitializeLevelOfSlowTurretActivity()
    {
        levelOfSlowTurretActivity = new bool[4];

        for (int i = 0; i < levelOfSlowTurretActivity.Length; i++)
        {
            levelOfSlowTurretActivity[i] = false;
        }
    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        //полоска жизни
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }
   

    public void ActivityTurret(int index, bool condition)
    {
        levelOfSlowTurretActivity[index] = condition;
    }

    public void Slow (float pct)
    {
        if (levelOfSlowTurretActivity[3])
        {
            speed = startSpeed * (1f - pct);
            return;
        }
        else if (levelOfSlowTurretActivity[2])
        {
            speed = startSpeed * (1f - pct);
            return;
        }
        else if (levelOfSlowTurretActivity[1])
        {
            speed = startSpeed * (1f - pct);
            return;
        }
        else if (levelOfSlowTurretActivity[0])
        {
            speed = startSpeed * (1f - pct);
            return;
        }
        else return;
        

    }

   public int CheckAliveHelpDrones()
    {
        int x = 0;
        if (enemyDrones == null) return 0;
        for (int i = 0; i < enemyDrones.Length; i++)
        {
            if(enemyDrones[i] != null)
            {
                x++;
            }
            

        }
        return x;
    }

    
    void Die ()
    {
        if (thisIsEnergyGenerator)
        {
            Shop.ShopClass.energyGeneratorAvailableCount ++;
        }


       audioSource.clip = destroySound;
        audioSource.Play();

        isDead = true;

        //прибавление денег за уничтожение
        PlayerStats.Money += worth;
        PlayerStats.Energy += worthEnergy;

        //эффект уничтожения
        GameObject effect = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        //счётчик количества врагов
        if (!thisIsMissile && !thisScriptOnTurret)
        {
            WaveSpawner.EnemiesAlive -= CheckAliveHelpDrones() + 1;
        }

        if (thisScriptOnTurret)
        {
            gameObject.GetComponent<Turret>().nodeToBuild.turretInstaled = false;
            gameObject.GetComponent<Turret>().nodeToBuild.isUpgraded = false;
            gameObject.GetComponent<Turret>().nodeToBuild.isUpgraded2 = false;
            gameObject.GetComponent<Turret>().nodeToBuild.isUpgraded3 = false;
        }

        Debug.Log("Враги: " + WaveSpawner.EnemiesAlive);

        

        Destroy(gameObject);
    }

    public void EndPath()
    {
        
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive -= CheckAliveHelpDrones() + 1;
        Destroy(gameObject);
        Debug.Log("Враги: " + WaveSpawner.EnemiesAlive);
    }

    public void FlyEnemySurviwe()
    {
        PlayerStats.Lives--;
        WaveSpawner.EnemiesAlive -= CheckAliveHelpDrones() + 1;
        Debug.Log("Враги: " + WaveSpawner.EnemiesAlive);
    }

    public void RepairTurret()
    {
        StartCoroutine("RepairHealth");
    }

    IEnumerator RepairHealth()
    {
        
        while (health < startHealth && repairHealthInProcess)
        {
            yield return new WaitForSeconds(1);
            

                if (PlayerStats.Money >= 1)
                {
                    PlayerStats.Money -= 1;
                    health += 10;
                }
                else
                {
                repairHealthInProcess = false;
                yield break;
                }
               
        }
       
        if (health > startHealth)
        {
            health = startHealth;
        }

        repairHealthInProcess = false;
        yield break;
    }

}

