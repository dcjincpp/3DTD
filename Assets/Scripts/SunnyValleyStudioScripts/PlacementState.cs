using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;

    int ID;

    Grid grid;

    PreviewSystem previewSystem;

    ObjectsDatabaseSO database;

    GridData enemyTileData;
    GridData playerTileData;

    ObjectPlacer objectPlacer;


    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData enemyTileData,
                          GridData playerTileData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.enemyTileData = enemyTileData;
        this.playerTileData = playerTileData;
        this.objectPlacer = objectPlacer;

        //go to database which is ObjectsDatabaseSO class, go to objectsData which is a list of ObjectData, find the ID that matches the inputted ID
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if(selectedObjectIndex > -1)
        {
            //if valid object index, start showing preview of selected object and cell indicator with appropriate size
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        } else {
            throw new System.Exception($"No object with ID {iD}");
        }
    }


    public void EndState ()
    {
        previewSystem.StopShowingPreview();
    }


    public void OnAction (Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValidity == false)
        {
            return;
        }

        //index is the number of objects created -1, returned by objectPlacer, like 5th object created returns 4
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //checks if the cell has an enemy tile
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState (Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

}
