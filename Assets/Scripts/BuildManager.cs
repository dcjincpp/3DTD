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

    [Header("Towers")]
    public GameObject standardTurretPrefab;
    public GameObject missileTurretPrefab;

    //what tower has been selected
    private TowerBlueprint turretToBuild;

    //checks if you have selected a tower. CanBuild = true when you have a tower selected
    public bool CanBuild { get { return turretToBuild != null; } }

    //build a turret on a node/tile
    public void BuildTurretOn (Node node)
    {
        //check if player has enough money
        if(PlayerResources.Money < turretToBuild.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        //take away tower cost from player money
        PlayerResources.Money -= turretToBuild.cost;

        //create turretToBuild's prefab on tiles position + offset and rotation
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        //tile now has tower on it
        node.turret = turret;

        Debug.Log("Turret built! Money left: " + PlayerResources.Money);
    }

    //stores tower put in parameter into towerToBuild, takes custom TowerBlueprint class
    public void SelectTurretToBuild (TowerBlueprint turret)
    {
        turretToBuild = turret;
    }
}
