using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    //gameobject that follows cursor
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private InputManager inputmanager;

    [SerializeField]
    private Grid grid;

    private void Update ()
    {
        Vector3 mousePosition = inputmanager.GetSelectedMapPosition();
        //converts world position of mouse to cell position
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        //mouseIndicator follows mouse inside cell, while cellIndicator stays until moved into new cell
        mouseIndicator.transform.position = mousePosition;
        //converts cell of grid position to world position, allows object to stay while mouse moves around inside cell due to int vector
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
