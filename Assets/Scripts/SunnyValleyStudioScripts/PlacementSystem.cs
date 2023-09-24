using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private GameObject end;

    [SerializeField]
    private InputManager inputmanager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    //placed flat tile
    private GridData enemyTileData; //can place tile under object but not on other tiles
    //placed objects on tile
    private GridData playerTileData; //can place objects on tile but not on other objects

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    //creates separate grid data for enemy tiles and player tiles so each can be placed on the same tile
    private void Start ()
    {
        StopPlacement();

        //grid data for placed enemy tiles
        enemyTileData = new();

        Vector3Int startPosition = Vector3Int.zero;

        int startBlockPosition = UnityEngine.Random.Range(1, 4);

        switch(startBlockPosition)
        {
            case 1:
                startPosition = new Vector3Int(GridData.spawnerPosition.x, 0, GridData.spawnerPosition.z + 1);
                break;
            
            case 2:
                startPosition = new Vector3Int(GridData.spawnerPosition.x, 0, GridData.spawnerPosition.z - 1);
                break;

            case 3:
                startPosition = new Vector3Int(GridData.spawnerPosition.x - 1, 0, GridData.spawnerPosition.z);
                break;
            
            case 4:
                startPosition = new Vector3Int(GridData.spawnerPosition.x + 1, 0, GridData.spawnerPosition.z);
                break;

        }



        //add initial path tile
        end.transform.position = grid.CellToWorld(startPosition);
        enemyTileData.AddObjectAt(startPosition, GridData.tileSize, 0, objectPlacer.PlaceEnemyTile(database.objectsData[0].Prefab, grid.CellToWorld(startPosition)));
        
        //grid data for placed player tiles
        playerTileData = new();
    }

    //start placement of object based on inputted ID, activates left click and escape, and shows preview
    public void StartPlacement(int ID)
    {
        StopPlacement();

        //show grid visual
        gridVisualization.SetActive(true);

        //create preview of selected object with its information and show cell indicator
        buildingState = new PlacementState(ID, grid, preview, database, enemyTileData, playerTileData, objectPlacer);

        //left clicking now places structure
        inputmanager.OnClicked += PlaceStructure;
        
        //clicking escape now stops placement
        inputmanager.OnExit += StopPlacement;
    }
    
    //start remove state 
    public void StartRemovement ()
    {
        StopPlacement();

        //show grid visual
        gridVisualization.SetActive(true);

        //create 1 x 1 cell indicator peview
        buildingState = new RemovingState(grid, preview, enemyTileData, playerTileData, objectPlacer);

        //left clicking now removes valid tiles
        inputmanager.OnClicked += PlaceStructure;

        //pressing escape stops remove state
        inputmanager.OnExit += StopPlacement;
    }

    //Creates or removes on gridPosition cell
    private void PlaceStructure()
    {
        if(inputmanager.IsPointerOverUI())
        {
            return;
        }

        //get position of mouse in world
        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();

        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //does the current building state's OnAction at the grid that the cursor is in, place or remove
        buildingState.OnAction(gridPosition);
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     //checks if the cell has an enemy tile
    //     GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

    //     return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    // }

    //stop placement system: turn off selected object preview if there is one, turns off grid visualization, sets click and escape to do nothing 
    private void StopPlacement()
    {
        //if not in a building state returns and does nothing
        if(buildingState == null)
        {
            return;
        }
        
        //turns off grid visuals
        gridVisualization.SetActive(false);

        //turn off cell indicator and destroy selected object preview if there is one
        buildingState.EndState();

        //left clicking no longer places/removes structures
        inputmanager.OnClicked -= PlaceStructure;
        //clicking escape no longer stops placement
        inputmanager.OnExit -= StopPlacement;

        //sets last detected position to (0, 0, 0)
        lastDetectedPosition = Vector3Int.zero;

        //sets building state from removing or placing to null
        buildingState = null;
    }

    //get cursor position and only moves cell indicator by cell when cursor moves into new cell on grid
    private void Update ()
    {
        //check if in building state
        if(buildingState == null)
        {
            return;
        }

        //get mouse position in world
        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();

        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        //if cursor moves into a new cell, update cell indicator and new cell cursor is in becomes old cell
        if(lastDetectedPosition != gridPosition)
        {
            //updateState checks validity, changes object preview and/or cell indicator color based on validity, and moves cell
            buildingState.UpdateState(gridPosition);
            
            Debug.Log("x = " + gridPosition.x + "y = " + gridPosition.y + "z = " + gridPosition.z);

            //new current position becomes old position
            lastDetectedPosition = gridPosition;

        }
    }
}
