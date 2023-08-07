using UnityEngine;

public class Shop : MonoBehaviour
{

    BuildManager buildManager;

    void Start ()
    {
        buildManager = BuildManager.instance;
    }
    public void PurchaseStandardTurret ()
    {
        Debug.Log("Standard Turret Purchased");
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);
    }

    public void PurchaseMissleTurret ()
    {
        Debug.Log("Missle Turret Purchased");
        buildManager.SetTurretToBuild(buildManager.missleTurretPrefab);

    }
}
