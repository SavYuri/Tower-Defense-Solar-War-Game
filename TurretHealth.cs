using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHealth : MonoBehaviour
{
    private float health;
    public float startHealth = 100;
    public Image healthBar;
    AudioSource audioSource;
    private bool isDead = false;
    public AudioClip destroySound;
    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarActivator();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        //полоска жизни
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        audioSource.clip = destroySound;
        audioSource.Play();
        isDead = true;
        //эффект уничтожения
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
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
}
