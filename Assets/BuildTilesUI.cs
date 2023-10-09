using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTilesUI : MonoBehaviour
{
    public GameObject tilesShopUI;
    public BuildTowersUI buildTowersUI;


    public void enableTilesShopUI ()
    {
        tilesShopUI.SetActive(true);
        buildTowersUI.disableTowerShopUI();
    }

    public void disableTilesShopUI ()
    {
        tilesShopUI.SetActive(false);
    }
}
