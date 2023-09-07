using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    public float startHealth = 100f;

    [HideInInspector]
    public float speed;
    [HideInInspector]
    private float health = 100f;

    public int worth = 25;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    void Start ()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow (float percent)
    {
        speed = startSpeed * (1f - percent);
    }

    void Die ()
    {
        Destroy(gameObject);
        
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);

        PlayerResources.Money += worth;
    }
}