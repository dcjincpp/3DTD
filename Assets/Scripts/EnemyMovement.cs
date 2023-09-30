using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    void Start ()
    {
        enemy = GetComponent<Enemy>();
        target = WayPoints.points[0];
    }

    void Update ()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }

    void GetNextWaypoint ()
    {
        Debug.Log("At waypoint" + waypointIndex);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Debug.Log("Total waypoints: " + (WayPoints.points.Count - 1));
        if (waypointIndex == WayPoints.points.Count - 1) //check if this is correct
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
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
