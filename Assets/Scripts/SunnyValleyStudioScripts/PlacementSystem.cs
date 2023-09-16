using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

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

    private void Start ()
    {
        StopPlacement();

        enemyTileData = new();
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

    public void StartRemovement ()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, enemyTileData, playerTileData, objectPlacer);
        inputmanager.OnClicked += PlaceStructure;
        inputmanager.OnExit += StopPlacement;
    }

    //Creates object to show what you chose but does not place
    private void PlaceStructure()
    {
        if(inputmanager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();
        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     //checks if the cell has an enemy tile
    //     GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

    //     return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    // }

    //stop placement system, turns off grid visualization, sets click and escape to do nothing, turn off preview 
    private void StopPlacement()
    {
        //if not in a building state returns and does nothing
        if(buildingState == null)
        {
            return;
        }
        

        gridVisualization.SetActive(false);
        buildingState.EndState();

        //left clicking no longer place structure
        inputmanager.OnClicked -= PlaceStructure;
        //clicking escape no longer stops placement
        inputmanager.OnExit -= StopPlacement;

        //sets last detected position to (0, 0, 0)
        lastDetectedPosition = Vector3Int.zero;

        //sets building state from removing or placing to null
        buildingState = null;
    }

    private void Update ()
    {
        if(buildingState == null)
        {
            return;
        }

        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();
        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {

            buildingState.UpdateState(gridPosition);

            lastDetectedPosition = gridPosition;
        }
    }
}
