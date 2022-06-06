
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Transform target;

    //скорость пули
    public float speed = 70f;

    public int damage = 50;

    //радиус поражения
    public float explosionRadios = 0f;

    public GameObject impactEffect;

    public Turret turret;

    bool searchTarget;

    public GameObject destroySelfPrefab;

    public bool SWMissle;

    //противовоздушная ракета
    public bool airMissle;

    public bool nuclearMissile;

    public bool enemyTurret;

    public AudioClip sWHitSound;

    AudioSource aSource;

    public bool enemyMissle;

    public GameObject trailEffect;
    Transform worldPivot;


    public void Seek (Transform _target)
    {
        target = _target;
    }
    void UpdateTarget()
    {

    }
    private void Start()
    {
        GameObject world = GameObject.Find("WorldPivot");
        worldPivot = world.transform;
        InvokeRepeating("DestroyIfStopSpeed", 1, 1);
        aSource = gameObject.GetComponent<AudioSource>();
       
                
        
    }
    // Update is called once per frame
    void Update()
    {
       
            BulletSearch();
        
        
        //newBulletSearch();
    }

    void newBulletSearch()
    {
        GameObject[] enemies;
        if (airMissle)
        {
           enemies = GameObject.FindGameObjectsWithTag("FlyEnemy");
        }
        else if (enemyMissle)
        {
            enemies = GameObject.FindGameObjectsWithTag("Turret");
        }
        else
        {
           enemies = GameObject.FindGameObjectsWithTag(turret.enemyTag);
        }
        
        if (enemies == null)
        {
           return;
        }

    }

    

    //ближайшая цель к пушке
    void BulletSearch()
    {
        //ракета ищет новую цель
        if (target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(turret.enemyTag);
            if (enemies == null)
            {
                if (trailEffect != null)
                {
                    trailEffect.transform.parent = worldPivot;
                }
                Destroy(gameObject);
                StartCoroutine(ExplodeEffect());
                return;
            }
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            if (enemies == null)
            {
                if (trailEffect != null)
                {
                    trailEffect.transform.parent = worldPivot;
                }
                Destroy(gameObject);
                StartCoroutine(ExplodeEffect());
                return;
            }
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            if (!SWMissle)
            {
                if (nearestEnemy != null && shortestDistance <= turret.range)
                {
                    target = nearestEnemy.transform;
                    turret.targetEnemy = nearestEnemy.GetComponent<Enemy>();
                    Seek(target);
                }
            }
            else
            {
                if (nearestEnemy != null)
                {
                    target = nearestEnemy.transform;
                    turret.targetEnemy = nearestEnemy.GetComponent<Enemy>();
                    Seek(target);
                }
            }
           
            //если цели нет, то самоуничтожение
            if (target == null)
            {
                if (trailEffect != null)
                {
                    trailEffect.transform.parent = worldPivot;
                }
                Destroy(gameObject);
                StartCoroutine(ExplodeEffect());

            }
        }
        if (target == null) return;


        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();

            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //что бы ракета смотрела по направлению к целе
        transform.LookAt(target);
    }



    Vector3 curPos;
    Vector3 lastPos;
    void DestroyIfStopSpeed()
    {
        curPos = transform.position;
        if (curPos == lastPos)
        {
            if (trailEffect != null)
            {
                trailEffect.transform.parent = worldPivot;
            }
            Destroy(gameObject);
        }
        lastPos = curPos;
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, Quaternion.identity);
       
        //уничтожает эффект обломков
        Destroy(effectIns, 3f);

        if (explosionRadios > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        //aSource.clip = sWHitSound;
        //aSource.Play();
        if(trailEffect != null)
        {
            trailEffect.transform.parent = worldPivot;
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

    //урон
    void Damage (Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }

    //отрисовка радиуса поражения при попадании снаряда
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadios);
    }

    IEnumerator ExplodeEffect()
    {
        Instantiate(destroySelfPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        Destroy(destroySelfPrefab);
        yield break;
    }
}
