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

        //go to database which is ObjectsDatabaseSO, go to objectsData which is a list of ObjectData, find the ID that matches the inputted ID
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);

        if(selectedObjectIndex > -1)
        {
            //if valid object index, start showing preview of selected object and cell indicator with appropriate size
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        } else {
            throw new System.Exception($"No object with ID {iD}");
        }
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
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        //return if gridPosition is not open
        if(placementValidity == false)
        {
            return;
        }

        //creates and places object in cell
        //index is the number of objects created -1, returned by objectPlacer, like 5th object created returns 4
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        //do i need this?
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

        //add object data to gridData
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

        //change preview indicator and preview object to red after placing since cell is now filled
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    //check if cell is open
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //checks if the cell has an enemy tile or player tile and selectedData becomes corresponding GridData, enemy or player
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;
        
        //true or false if object can be placed in corresponding GridData
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    //used to move cell indicator and object preview and change color in PlacementSystem (both states have same function why not just remove from interface and put in placement system?)
    public void UpdateState (Vector3Int gridPosition)
    {
        //check if you can place
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        //moves object and cell indicator and changes color based on validity
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

}
