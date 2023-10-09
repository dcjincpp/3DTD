using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowersUI : MonoBehaviour
{
    public GameObject shopBar;
    public PlacementSystem placementSystem;
    public BuildTilesUI buildTilesUI;

    public void enableTowerShopUI ()
    {
        shopBar.SetActive(true);
        buildTilesUI.disableTilesShopUI();
        placementSystem.StopPlacement();
    }

    public void disableTowerShopUI ()
    {
        shopBar.SetActive(false);
    }
}
