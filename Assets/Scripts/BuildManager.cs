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
    public GameObject missileTurretPrefab;
    private TowerBlueprint turretToBuilid;

    public bool CanBuild { get { return turretToBuilid != null; } }

    public void BuildTurretOn (Node node)
    {
        if(PlayerResources.Money < turretToBuilid.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        PlayerResources.Money -= turretToBuilid.cost;

        GameObject turret = (GameObject)Instantiate(turretToBuilid.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        Debug.Log("Turret built! Money left: " + PlayerResources.Money);
    }

    public void SelectTurretToBuild (TowerBlueprint turret)
    {
        turretToBuilid = turret;
    }
}
