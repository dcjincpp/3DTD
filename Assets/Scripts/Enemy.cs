using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public float health = 100f;
    public int worth = 25;

    public GameObject deathEffect;

    private Transform target;
    private int waypointIndex = 0;

    void Start ()
    {
        target = WayPoints.points[0];
    }

    public void TakeDamage (float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Destroy(gameObject);
        
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);

        PlayerResources.Money += worth;
    }

    void Update ()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint ()
    {
        if (waypointIndex > WayPoints.points.Length)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = WayPoints.points[waypointIndex];
    }

    void EndPath ()
    {
        PlayerResources.Lives--;
        Destroy(gameObject);
    }
}