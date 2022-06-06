using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBullet : MonoBehaviour
{
    public Transform target;

    //скорость пули
    public float speed = 70f;

    public int damage = 50;

    //радиус поражения
    public float explosionRadios = 0f;

    public GameObject impactEffect;

    public Turret turret;

    public GameObject destroySelfPrefab;

    Vector3 lastEnemyPosition;
    Transform lastEnemyTransform;
    public GameObject trailEffect;
    Transform worldPivot;



    // Start is called before the first frame update
    void Start()
    {
        GameObject world = GameObject.Find("WorldPivot");
        worldPivot = world.transform;
        AudioSource Au = gameObject.GetComponent<AudioSource>();
        Au.Play();
       
        //SearchTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //рисует область поражения ракеты
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadios);
    }

    void SearchTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(turret.enemyTag);
        if (enemies == null)
        {
            this.enabled = false;
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(turret.transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= turret.range)
        {
            target = nearestEnemy.transform;
            turret.targetEnemy = nearestEnemy.GetComponent<Enemy>();
            Seek(target);
            AudioSource au = gameObject.GetComponent<AudioSource>();
            au.Play();
        }
        else
        {
            this.enabled = false;
        }

       

    }

   

    void Movement()
    {
        if (target == null)
        {
            goToLastPosition();
           return;

        }

        Vector3 dir = target.position - transform.position;
        lastEnemyPosition = target.position;
        
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

    void goToLastPosition()
    {
        if (lastEnemyPosition == new Vector3 (0,0,0))
        {
            if (trailEffect != null)
            {
                trailEffect.transform.parent = worldPivot;
            }
            Destroy(gameObject);
        }

        Vector3 dir = lastEnemyPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {

            if (explosionRadios > 0f)
            {
                Explode();
            }
            if (trailEffect != null)
            {
                trailEffect.transform.parent = worldPivot;
            }
            Destroy(gameObject);

            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //что бы ракета смотрела по направлению к целе
        transform.LookAt(lastEnemyTransform);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, Quaternion.identity);

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
        if (trailEffect != null)
        {
            trailEffect.transform.parent = worldPivot;
        }
        //уничтожает пулю
        Destroy(gameObject);
    }


    //урон
    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
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


    IEnumerator ExplodeEffect()
    {
        Instantiate(destroySelfPrefab, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        Destroy(destroySelfPrefab);
        yield break;
    }


    public void Seek(Transform _target)
    {
        target = _target;
    }
}
