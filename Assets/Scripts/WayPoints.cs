using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public GameObject wayPointPrefab;

    public static Transform[] points;

    public void getEnemyPathing()
    {
        points = new Transform[transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

    public GameObject createWaypoint (Vector3 gridPosition)
    {
        //create new waypoint and make into child of object script is attached to
        GameObject newWaypoint = Instantiate(wayPointPrefab, this.transform);

        //move waypoint to inputted gridposition
        newWaypoint.transform.position = gridPosition;

        getEnemyPathing();

        return newWaypoint;
    }

    public void removeWaypoint ()
    {
        Destroy(points[points.Length - 1].gameObject);

        getEnemyPathing();
    }
}
