using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start ()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //create prefab of selected object to place, put preview material on prefab and  cell indicator, activate cell indicator
    public void StartShowingPlacementPreview (GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);

        PreparePreview(previewObject);
        PrepareCursor(size);

        cellIndicator.SetActive(true);
    }

    //put transparent preview material on all of gameObjects materials
    private void PreparePreview (GameObject previewObject)
    {
        //get array of renderer components in prefab
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

        //go through array of renderers
        foreach(Renderer renderer in renderers)
        {
            //array of materials being rendered
            Material[] materials = renderer.materials;

            //replace material with transparent preview material
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }

            //replace materials in renderer with materials array (unsure)
            renderer.materials = materials;
        }
    }


    private void PrepareCursor (Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    public void StopShowingPreview ()
    {
        cellIndicator.SetActive(false);
        if(previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    public void UpdatePosition (Vector3 position, bool validity)
    {
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
    }

    internal void StartShowingRemovePreviwe()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
