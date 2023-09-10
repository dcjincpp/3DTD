using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;
    
    [SerializeField]
    //what layer the ray can hit
    private LayerMask placementLayerMask;

    public Vector3 GetSelectedMapPosition ()
    {
        //x and y position of mouse from origin (bottom left of screen), z is 0
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
