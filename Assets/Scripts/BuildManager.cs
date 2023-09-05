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

    public GameObject buildEffect;

    //what tower has been selected
    private TowerBlueprint turretToBuild;
    private Node selectedNode;

    //checks if you have selected a tower. CanBuild = true when you have a tower selected
    public bool CanBuild { get { return turretToBuild != null; } }
    //checks if you have enough money to build turretToBuild (selected turret to build)
    public bool HasMoney { get { return PlayerResources.Money >= turretToBuild.cost; } }

    public NodeUI nodeUI;

    public void SelectNode (Node node)
    {
        if(selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    //stores tower put in parameter into towerToBuild, takes custom TowerBlueprint class
    public void SelectTurretToBuild (TowerBlueprint turret)
    {
        turretToBuild = turret;
        
        DeselectNode();
    }

    public TowerBlueprint GetTurretToBuild ()
    {
        return turretToBuild;
    }
}
