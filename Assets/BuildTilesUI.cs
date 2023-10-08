using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTilesUI : MonoBehaviour
{
    public GameObject tilesShopUI;

    public void enableTilesShopUI ()
    {
        tilesShopUI.SetActive(true);
    }

    public void disableTilesShopUI ()
    {
        tilesShopUI.SetActive(false);
    }
}
