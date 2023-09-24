using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps track of objects created
public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedEnemyTiles = new();
    int latestEnemyTileIndex = -1;

    [SerializeField]
    private List<GameObject> placedPlayerTiles = new();

    public int PlacePlayerTile (GameObject prefab, Vector3 position)
    {
        //creates prefab connected to object ID in database
        GameObject selectedObject = Instantiate(prefab);

        //puts created object in cell
        selectedObject.transform.position = position;

        //add created object into list of placed objects
        placedPlayerTiles.Add(selectedObject);

        //return how many objects placed -1
        return placedPlayerTiles.Count - 1; //why start at 0? change?, because index starts at 0
    }

    public int PlaceEnemyTile (GameObject prefab, Vector3 position)
    {
        //creates prefab connected to object ID in database
        GameObject selectedObject = Instantiate(prefab);

        //puts created object in cell
        selectedObject.transform.position = position;

        //add created object into list of placed objects
        placedEnemyTiles.Add(selectedObject);
        latestEnemyTileIndex++;

        //return how many objects placed -1
        return placedEnemyTiles.Count - 1; //why start at 0? change?, because index starts at 0
    }

    internal void RemovePlayerTile (int gameObjectIndex)
    {
        //if the amount of placed objects on the tile is less than or equal to the number of objects -1 or there is no gameObject with index in list, return
        if(placedPlayerTiles.Count <= gameObjectIndex || placedPlayerTiles[gameObjectIndex] == null)
        {
            return;
        }

        //destroy gameobject with index
        Destroy(placedPlayerTiles[gameObjectIndex]);

        //replace list at index with null
        placedPlayerTiles[gameObjectIndex] = null;
    }

    internal void RemoveEnemyTile (int gameObjectIndex)
    {
        //if the amount of placed objects on the tile is less than or equal to the number of objects -1 or there is no gameObject with index in list, return
        if(placedEnemyTiles.Count <= gameObjectIndex || placedEnemyTiles[gameObjectIndex] == null)
        {
            return;
        }

        latestEnemyTileIndex--;
        
        //destroy gameobject with index
        Destroy(placedEnemyTiles[gameObjectIndex]);

        // Debug.Log("Index of item being removed: " + gameObjectIndex);
        // Debug.Log("Index of latest enemy tile: " + latestEnemyTileIndex);

        //replace list at index with null
        placedEnemyTiles.RemoveAt(gameObjectIndex);
    }

    public int GetLatestEnemyTileIndex ()
    {
        return latestEnemyTileIndex;
    }
}
