using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectID = - 1;
    Dictionary<Vector3Int, PlacementData> enemyTileGridData;

    int ID;

    Grid grid;

    PreviewSystem previewSystem;

    ObjectsDatabaseSO database;

    GridData enemyTileData;
    GridData playerTileData;

    ObjectPlacer objectPlacer;

    GameObject endTile;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData enemyTileData,
                          GridData playerTileData,
                          ObjectPlacer objectPlacer,
                          GameObject endTile)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.enemyTileData = enemyTileData;
        this.playerTileData = playerTileData;
        this.objectPlacer = objectPlacer;
        this.endTile = endTile;

        //go to database which is ObjectsDatabaseSO, go to objectsData which is a list of ObjectData, find the ID that matches the inputted ID
        selectedObjectID = database.objectsData.FindIndex(data => data.ID == ID);

        if(selectedObjectID > -1)
        {
            //if valid object index, start showing preview of selected object and cell indicator with appropriate size
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectID].Prefab, database.objectsData[selectedObjectID].Size);
        } else {
            throw new System.Exception($"No object with ID {iD}");
        }

        enemyTileGridData = enemyTileData.GetPlacedObjectsDictionary();

    }

    //stop showing preview cell indicator and object
    public void EndState ()
    {
        previewSystem.StopShowingPreview();
    }

    //place object
    public void OnAction (Vector3Int gridPosition)
    {

        //check if gridPosition is open
        //return if gridPosition is not open
        if(CheckPlacementValidity(gridPosition, selectedObjectID) == false)
        {
            return;
        }

        GridData selectedData;
        int index;

        //do i need this?
        if(database.objectsData[selectedObjectID].ID == 0)
        {
            selectedData = enemyTileData;

            //creates and places object in cell
            //index is the number of objects created -1, returned by objectPlacer, like 5th object created returns 4
            index = objectPlacer.PlaceEnemyTile(database.objectsData[selectedObjectID].Prefab, grid.CellToWorld(gridPosition));

            //move end tile to new placed enemy tile
            endTile.transform.position = grid.CellToWorld(gridPosition);

        } else {
            selectedData = playerTileData;
            //creates and places object in cell
            //index is the number of objects created -1, returned by objectPlacer, like 5th object created returns 4
            index = objectPlacer.PlacePlayerTile(database.objectsData[selectedObjectID].Prefab, grid.CellToWorld(gridPosition));
        }

        //add object data to gridData
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectID].Size, database.objectsData[selectedObjectID].ID, index);
        
        //change preview indicator and preview object to red after placing
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    //check if cell is open
    private bool CheckPlacementValidity (Vector3Int gridPosition, int selectedObjectID)
    {
        if(gridPosition == GridData.spawnerPosition)
        {
            return false;
        }

        //checks if the cell has an enemy tile or player tile and selectedData becomes corresponding GridData, enemy or player
        GridData selectedData;

        if(database.objectsData[selectedObjectID].ID == 0)
        {
            selectedData = enemyTileData;

            if(!NextToLatestTile(gridPosition))
            {
                return false;
            }

        } else {
            selectedData = playerTileData;
        }
        
        //true or false if object can be placed in corresponding GridData
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectID].Size);
    }

    //used to move cell indicator and object preview and change color in PlacementSystem
    public void UpdateState (Vector3Int gridPosition)
    {
        //check if you can place
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectID);

        //moves object and cell indicator and changes color based on validity
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    bool NextToLatestTile (Vector3Int gridPosition)
    {
        Vector3Int up = new Vector3Int(gridPosition.x, 0, gridPosition.z + 1);
        Vector3Int down = new Vector3Int(gridPosition.x, 0, gridPosition.z - 1);
        Vector3Int left = new Vector3Int(gridPosition.x - 1, 0, gridPosition.z);
        Vector3Int right = new Vector3Int(gridPosition.x + 1, 0, gridPosition.z);
        Dictionary<Vector3Int, PlacementData> enemyTileGridData = enemyTileData.GetPlacedObjectsDictionary();

            // Debug.Log("Index of tile on the right: " + enemyTileGridData[right].PlacedObjectIndex);
            // Debug.Log("Index of latest tile: " + objectPlacer.GetLatestEnemyTileIndex());
            // Debug.Log("latest tile is on the right");

        if(enemyTileGridData.ContainsKey(up) && (enemyTileGridData[up].PlacedObjectIndex == objectPlacer.GetLatestEnemyTileIndex()))
        {
            Debug.Log(enemyTileGridData[up].PlacedObjectIndex + " = " + objectPlacer.GetLatestEnemyTileIndex());
            Debug.Log("latest tile is up");
            return true;
        } else if(enemyTileGridData.ContainsKey(down) && (enemyTileGridData[down].PlacedObjectIndex == objectPlacer.GetLatestEnemyTileIndex()))
        {
            Debug.Log(enemyTileGridData[down].PlacedObjectIndex + " = " + objectPlacer.GetLatestEnemyTileIndex());
            Debug.Log("latest tile is below");
            return true;
        } else if(enemyTileGridData.ContainsKey(left) && (enemyTileGridData[left].PlacedObjectIndex == objectPlacer.GetLatestEnemyTileIndex()))
        {
            Debug.Log(enemyTileGridData[left].PlacedObjectIndex + " = " + objectPlacer.GetLatestEnemyTileIndex());
            Debug.Log("latest tile is on the left");
            return true;
        } else if(enemyTileGridData.ContainsKey(right) && (enemyTileGridData[right].PlacedObjectIndex == objectPlacer.GetLatestEnemyTileIndex()))
        {
            Debug.Log(enemyTileGridData[right].PlacedObjectIndex + " = " + objectPlacer.GetLatestEnemyTileIndex());
            Debug.Log("latest tile is on the right");
            return true;
        }

        return false;
    }

}
