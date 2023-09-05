using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBlueprint
{
    public GameObject prefab;
    public int cost;
    [Space(10)]
    public GameObject upgradedPrefab;
    public int upgradeCost;
}
