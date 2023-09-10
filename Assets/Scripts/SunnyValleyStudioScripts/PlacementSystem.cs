using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    //gameobject that follows cursor
    private GameObject mouseIndicator;
    [SerializeField]
    private GameObject cellIndicator;

    [SerializeField]
    private InputManager inputmanager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private void Start ()
    {
        StopPlacement();
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
        cellIndicator.SetActive(true);
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
        //creates prefab connected to object ID in database
        GameObject selectedObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        //converts cell of grid position to world position, allows object to stay while mouse moves around inside cell due to int vector
        selectedObject.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputmanager.OnClicked -= PlaceStructure;
        inputmanager.OnExit -= StopPlacement;
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
        //mouseIndicator follows mouse inside cell, while cellIndicator stays until moved into new cell
        mouseIndicator.transform.position = mousePosition;
        //converts cell of grid position to world position, allows object to stay while mouse moves around inside cell due to int vector
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
