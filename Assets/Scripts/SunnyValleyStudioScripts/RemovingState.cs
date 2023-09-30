using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;

    Dictionary<Vector3Int, PlacementData> enemyTileGridData;

    Grid grid;

    PreviewSystem previewSystem;

    //start at null and switches based on item removing
    GridData selectedData;

    GridData enemyTileData;
    GridData playerTileData;

    ObjectPlacer objectPlacer;

    GameObject endTile;

    public RemovingState( Grid grid,
                          PreviewSystem previewSystem,
                          GridData enemyTileData,
                          GridData playerTileData,
                          ObjectPlacer objectPlacer,
                          GameObject endTile)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.enemyTileData = enemyTileData;
        this.playerTileData = playerTileData;
        this.objectPlacer = objectPlacer;
        this.endTile = endTile;

        enemyTileGridData = enemyTileData.GetPlacedObjectsDictionary();

        //remove preview, red on empty tiles, white on occupied tiles
        previewSystem.StartShowingRemovePreview();
    }

    //stop showing preview cell indicator and object
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    //remove object
    public void OnAction(Vector3Int gridPosition)
    {
        if(gridPosition == GridData.spawnerPosition)
        {
            return;
        }
        
        //dont care about size because removing
        //check if occupied by playerTile
        if(playerTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            //go into playerTile GridData
            selectedData = playerTileData;
            
          //check if occupied by enemyTile
        } else if(enemyTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            //go into enemyTile GridData
            selectedData = enemyTileData;
        }

        //if nothing is there
        if(selectedData == null)
        {
            return;
          
          //remove object or enemy tile
        } else if (selectedData == enemyTileData)
        {   
            //what number object it is
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            //if -1 means there is no object in the GridData
            if((gameObjectIndex == -1) || (gameObjectIndex != objectPlacer.GetLatestEnemyTileIndex()) || gameObjectIndex == 0)
            {
                return;
            }

            //remove object GridData
            selectedData.RemoveObjectAt(gridPosition);

            //destroy gameObject
            objectPlacer.RemoveEnemyTile(gameObjectIndex);

            //move end object to tile before removed tile
            endTile.transform.position = objectPlacer.GetPreviousEnemyTilePosition();

        } else if (selectedData == playerTileData)
        {   //what number object it is
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            //if -1 means there is no object in the GridData
            if(gameObjectIndex == -1)
            {
                return;
            }

            //remove object GridData
            selectedData.RemoveObjectAt(gridPosition);

            //destroy gameObject
            objectPlacer.RemovePlayerTile(gameObjectIndex);
        } 

        //world position of cell
        Vector3 cellPosition = grid.CellToWorld(gridPosition);

        //check cell for validity and change color of cell indicator accordingly after you remove
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    //check if there is something to remove
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        //checks if there is a playertile or enemytile
        return !(playerTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && enemyTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    //used to move cell indicator and object preview and change color in PlacementSystem
    public void UpdateState (Vector3Int gridPosition)
    {
        bool validity;

        if((gridPosition == GridData.spawnerPosition) || (enemyTileGridData.ContainsKey(gridPosition) && (enemyTileGridData[gridPosition].PlacedObjectIndex == 0 || (enemyTileGridData[gridPosition].PlacedObjectIndex != objectPlacer.GetLatestEnemyTileIndex()))))
        {
            validity = false;
        } else {
            //check if there is something to remove
            validity = CheckIfSelectionIsValid(gridPosition);
        }
        
        //moves cell indicator and changes color based on validity
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
