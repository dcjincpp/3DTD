using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public GameObject wayPointPrefab;

    public static List<Transform> points = new();

    public void getEnemyPathing()
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

    public GameObject createWaypoint (Vector3 gridPosition)
    {
        //create new waypoint and make into child of object script is attached to
        GameObject newWaypoint = Instantiate(wayPointPrefab, this.transform);

        points.Add(newWaypoint.transform);

        //move waypoint to inputted gridposition
        newWaypoint.transform.position = gridPosition;

        getEnemyPathing();

        return newWaypoint;
    }

    public void removeWaypoint ()
    {
        Destroy(points[points.Count - 1].gameObject);

        points.RemoveAt(points.Count - 1);

        getEnemyPathing();
    }
}
