using UnityEngine;

//visual indicator of cursor, object, and theirplacement validity
public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    //referenced by previewMaterialInstance
    private Material previewMaterialsPrefab;
    
    //used in code for material
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start ()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    //create prefab of selected object to place, put preview material on prefab and cell indicator, activate cell indicator
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

    //change size of cell indicator if object is larger than 0
    private void PrepareCursor (Vector2Int size)
    {
        if(size.x > 0 || size.y > 0)
        {
            //change size of indicator
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            
            //change size of indicator texture
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    //stop showing build preview
    public void StopShowingPreview ()
    {
        //turn off cell indicator
        cellIndicator.SetActive(false);

        //destroy preview of selected object
        if(previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    //move selected object's preview and cell indicator to cursor
    public void UpdatePosition (Vector3 position, bool validity)
    {
        //if object selected, move it to position of cell that the cursor is in and apply color based on validity
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        //move cell indicator to position of cell that the cursor is in and apply color based on validity
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    //move selected object's preview to position, which is cursor position
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    //move cell indicator to position, which is cursor position
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    //change selected object's preview to transparent red/white based on validity
    private void ApplyFeedbackToPreview(bool validity)
    {
        //if valid white, else red
        Color c = validity ? Color.white : Color.red;

        //color alpha
        c.a = 0.5f;

        //set selected object's color to transparent white or red depending on validity
        previewMaterialInstance.color = c;

    }

    //change cell indicator to transparent red/white based on validity
    private void ApplyFeedbackToCursor(bool validity)
    {
        //if valid white, else red
        Color c = validity ? Color.white : Color.red;

        //color alpha
        c.a = 0.5f;

        //set cellindicators color to transparent white or red depending on validity
        cellIndicatorRenderer.material.color = c;

    }

    //remove option preview
    internal void StartShowingRemovePreview()
    {
        //activate cell indicator
        cellIndicator.SetActive(true);

        //size of cell indicator is 1 x 1
        PrepareCursor(Vector2Int.one);

        //red as default because removing, becomes valid/white when over a created tile
        ApplyFeedbackToCursor(false);
    }
}
