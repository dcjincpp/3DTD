using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    //gameobject that follows cursor
    private GameObject mouseIndicator;

    [SerializeField]
    private InputManager inputmanager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    //placed flat tile
    private GridData enemyTileData; //can place tile under object but not on other tiles
    //placed objects on tile
    private GridData playerTileData; //can place objects on tile but not on other objects

    private List<GameObject> placedGameobjects = new();

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start ()
    {
        StopPlacement();

        enemyTileData = new();
        playerTileData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        //go to database which is ObjectsDatabaseSO class, go to objectsData which is a list of ObjectData, find the ID that matches the inputted ID
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.Log("clicked 2");
            Debug.LogError($"No ID Found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        inputmanager.OnClicked += PlaceStructure;
        inputmanager.OnExit += StopPlacement;
        Debug.Log("clicked");
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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValidity == false)
        {
            return;
        }

        //creates prefab connected to object ID in database
        GameObject selectedObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //converts cell of grid position to world position, allows object to stay while mouse moves around inside cell due to int vector
        selectedObject.transform.position = grid.CellToWorld(gridPosition);

        placedGameobjects.Add(selectedObject);

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameobjects.Count - 1);

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        //checks if the cell has an enemy tile
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? enemyTileData : playerTileData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();

        inputmanager.OnClicked -= PlaceStructure;
        inputmanager.OnExit -= StopPlacement;

        lastDetectedPosition = Vector3Int.zero;
    }

    private void Update ()
    {
        if(selectedObjectIndex < 0)
        {
            return;
        }

        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();
        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            //mouseIndicator follows mouse inside cell, while cellIndicator stays until moved into new cell
            mouseIndicator.transform.position = mousePosition;

            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);

            lastDetectedPosition = gridPosition;
        }
    }
}
