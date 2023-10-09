using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    public Color hoverColor;
    public Color invalidColor;
    public Vector3 positionOffset = new Vector3(0f, 0.5f, 0f);

    [HideInInspector]
    //what turret is occupying the tile
    public GameObject turret;

    [HideInInspector]
    public TowerBlueprint towerBlueprint;

    [HideInInspector]
    public bool isUpgraded = false;

    public Renderer rend;
    private Color startColor;
    
    BuildManager buildManager;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    //get position of built tower on node
    public Vector3 GetBuildPosition ()
    {
        return transform.position + positionOffset;
    }

    void OnMouseUpAsButton()
    {
        //if clicked a ui element dont click through, return
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        BuildTower(buildManager.GetTurretToBuild());
    }

    //build a turret on a node/tile
    void BuildTower (TowerBlueprint tower)
    {
        //check if player has enough money
        if(PlayerResources.Money < tower.cost)
        {
            Debug.Log("Not enough money");
            return;
        }

        //take away tower cost from player money
        PlayerResources.Money -= tower.cost;

        //create turretToBuild's prefab on tiles position + offset and rotation
        GameObject _turret = (GameObject)Instantiate(tower.prefab, GetBuildPosition(), Quaternion.identity);

        towerBlueprint = tower;

        //tile now has tower on it
        turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 3f);

        Debug.Log("Turret built!");

        rend.material.color = invalidColor;

    }

    public void UpgradeTurret ()
    {
        //check if player has enough money
        if(PlayerResources.Money < towerBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money");
            return;
        }

        //destroy turret currently on tile (previous version of tower)
        Destroy(turret);

        //take away tower upgrade cost from player money
        PlayerResources.Money -= towerBlueprint.upgradeCost;

        //create turretToBuild's upgraded prefab on tiles position + offset and rotation
        GameObject _turret = (GameObject)Instantiate(towerBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);

        //tile now has upgraded tower on it from previous line ^^^
        turret = _turret;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 3f);

        isUpgraded = true;

        Debug.Log("Turret upgraded!");
    }

    public void SellTurret ()
    {
        PlayerResources.Money += towerBlueprint.GetSellValue();

        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 3f);

        Destroy(turret);
        isUpgraded = false;
        towerBlueprint = null;
    }

    void OnMouseEnter ()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (buildManager.HasMoney && turret == null)
        {
            rend.material.color = hoverColor;
        } else {
            rend.material.color = invalidColor;
        }
    }

    void OnMouseExit ()
    {
        rend.material.color = startColor;
    }

    public float getTurretRange ()
    {
        return turret.GetComponent<Turret>().range;
    }

    public void resetColor ()
    {
        rend.material.color = startColor;
    }
}
