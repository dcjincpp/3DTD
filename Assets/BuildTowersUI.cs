using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowersUI : MonoBehaviour
{
    public GameObject shopBar;
    public PlacementSystem placementSystem;

    public void enableShop ()
    {
        shopBar.SetActive(true);
        placementSystem.StopPlacement();
    }

    public void disableShop ()
    {
        shopBar.SetActive(false);
    }
}
