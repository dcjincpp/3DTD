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

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID, grid, preview, database, enemyTileData, playerTileData, objectPlacer);

        inputmanager.OnClicked += PlaceStructure;
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

    private void StopPlacement()
    {
        if(buildingState == null)
        {
            return;
        }
        
        gridVisualization.SetActive(false);
        buildingState.EndState();

        inputmanager.OnClicked -= PlaceStructure;
        inputmanager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;

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
