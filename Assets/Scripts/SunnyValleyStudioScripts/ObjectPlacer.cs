using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameobjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        //creates prefab connected to object ID in database
        GameObject selectedObject = Instantiate(prefab);

        //puts created object in cell
        selectedObject.transform.position = position;

        //add created object into list of placed objects
        placedGameobjects.Add(selectedObject);

        //return how many objects placed -1
        return placedGameobjects.Count - 1; //why start at 0? change?
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        //if the amount of placed objects on the tile is less than or equal to the
        if(placedGameobjects.Count <= gameObjectIndex || placedGameobjects[gameObjectIndex] == null)
        {
            return;
        }

        Destroy(placedGameobjects[gameObjectIndex]);
        placedGameobjects[gameObjectIndex] = null;
    }
}
