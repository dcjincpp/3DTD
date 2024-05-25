using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter (Collider collider)
    {
        GameObject enteredObject = collider.gameObject;

        if(enteredObject.name == "Cube")
        {
            Debug.Log("entered");
            Destroy(enteredObject);
        }
    }
}
