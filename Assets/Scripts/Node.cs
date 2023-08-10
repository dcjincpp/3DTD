using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{

    public Color hoverColor;
    public Color invalidColor;
    public Vector3 positionOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Optional")]
    //what turret is occupying the tile
    public GameObject turret;

    private Renderer rend;
    private Color startColor;
    
    BuildManager buildManager;

    void Start ()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition ()
    {
        return transform.position + positionOffset;
    }

    void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (turret != null)
        {
            Debug.Log("Already something on tile (display on screen)");
            return;
        }

        buildManager.BuildTurretOn(this);
    }

    void OnMouseEnter ()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        } else {
            rend.material.color = invalidColor;
        }
    }

    void OnMouseExit ()
    {
        rend.material.color = startColor;
    }
}
