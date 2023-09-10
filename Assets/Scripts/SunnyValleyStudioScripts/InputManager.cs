using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;
    
    [SerializeField]
    //what layer the ray can hit
    private LayerMask placementLayerMask;

    public event Action OnClicked;
    public event Action OnExit;

    public void Update ()
    {
        //left mouse button
        if(Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI ()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition ()
    {
        //x and y position of mouse from origin (bottom left of screen), z is 0
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        //ray that starts from screen to given point in parameter (mousePos)
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
