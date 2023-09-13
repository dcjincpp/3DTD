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
        //converts cell of grid position to world position, allows object to stay while mouse moves around inside cell due to int vector
        selectedObject.transform.position = position;

        placedGameobjects.Add(selectedObject);

        return placedGameobjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if(placedGameobjects.Count <= gameObjectIndex || placedGameobjects[gameObjectIndex] == null)
        {
            return;
        }
        Destroy(placedGameobjects[gameObjectIndex]);
        placedGameobjects[gameObjectIndex] = null;
    }
}
