using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.Rendering;
using UnityEngine;

//keeps track of what cells are being taken up
public class GridData
{

    public static Vector3Int spawnerPosition = new Vector3Int(0, 0, 0);
    public static Vector2Int tileSize = new Vector2Int(1, 1);


    //dictionary of placed object key is cell position, definition is data of what is on cell position
    Dictionary<Vector3Int, PlacementData> placedObjects = new(); //cell position is key, return placement data

    //add object
    public void AddObjectAt(Vector3Int gridPosition, //where the object is in the cell
                            Vector2Int objectSize, //size of object
                            int ID, //what object
                            int placedObjectIndex) //what number the object created is
    {
        //where and how many cells object takes up
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        
        //create new placement data: record where and how many cells it is on, what object, and what number the object created is
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        //check if the cells the object occupies is over another occupied cell
        foreach(var pos in positionToOccupy)
        {
            //if placed object is in another placed objects cell, throw exception
            if(placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains cell position {pos}");
            }

            //if nothing wrong, create the objects data in the positions it takes up
            placedObjects[pos] = data;
        }

    }

    //calculate cells that the object takes up
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //list of cells being taken up
        List<Vector3Int> returnVal = new();

        //x of size
        for(int x = 0; x < objectSize.x; x++)
        {
            //y of size
            for(int y = 0; y< objectSize.y; y++)
            {
                //add 1 cell at x, y
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        
        //return cells that the object takes up
        return returnVal;
    }

    //check if you can place object at position
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //calculate positions the object would take up /*edited out calculating object size position to occupy because my tiles are 1x1*/
        //List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        //if part of object is in a position that has placement data, can not place and return false else true
        //foreach(var pos in positionToOccupy)
        //{
            if(placedObjects.ContainsKey(gridPosition))
            {
                return false;
            }
        //}

        return true;
    }


    //if there is a reference to the key at the gridposition, return the reference index
    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        //if nothing is at gridPosition, return -1
        if(placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }

        //return index in PlacementData of placed object on gridPosition
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    //remove PlacementData of object connected to gridPosition in dictionary so that it becomes a free cell(s)
    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        //for each occupied cell of the object on gridPosition, remove the placementData of the object off every cell from placedObjects dictionary
        foreach(var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }

    //getter for placedObjects dictionary
    public Dictionary<Vector3Int, PlacementData> GetPlacedObjectsDictionary ()
    {
        return placedObjects;
    }
}

//class to keep track of which cells have been taken up and by what and index (what number object)
//creates a list for every object
public class PlacementData
{
    //list of occupied positions
    public List<Vector3Int> occupiedPositions;

    //what object
    public int ID {get; private set;}

    //what number object
    public int PlacedObjectIndex {get; private set;}

    //where, what object, what number object
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}