using UnityEngine;

public class BuildManager : MonoBehaviour
{
    
    public static BuildManager instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.Log("More than one buildmanager in scene");
            return;
        }
        instance = this;
    }

    public GameObject standardTurretPrefab;
    public GameObject anotherTurretPrefab;
    private GameObject turretToBuilid;

    public GameObject GetTurretToBuild ()
    {
        return turretToBuilid;
    }
    
    public void SetTurretToBuild (GameObject turret)
    {
        turretToBuilid = turret;
    }
}
