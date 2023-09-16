using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;

    Grid grid;

    PreviewSystem previewSystem;

    GridData enemyTileData;
    GridData playerTileData;

    ObjectPlacer objectPlacer;

    public RemovingState( Grid grid,
                          PreviewSystem previewSystem,
                          GridData enemyTileData,
                          GridData playerTileData,
                          ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.enemyTileData = enemyTileData;
        this.playerTileData = playerTileData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreviwe();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        //dont care about size because removing
        if(playerTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = playerTileData;
        } else if(enemyTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = enemyTileData;
        }

        if(selectedData == null)
        {

        } else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            if(gameObjectIndex == -1)
            {
                return;
            }

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(playerTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one) && enemyTileData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
