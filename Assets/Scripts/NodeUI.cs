using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject rangeIndicator;
    public GameObject uI;
    private Node target;
    public Button upgradeButton;

    public TMPro.TextMeshProUGUI upgradeCost;
    public TMPro.TextMeshProUGUI sellValue;

    public void SetTarget (Node _target)
    {
        target = _target;

        target.resetColor();

        transform.position = target.GetBuildPosition();

        if(!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.towerBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        } else 
        {
            upgradeButton.interactable = false;
            upgradeCost.text = "MAXED";
        }

        sellValue.text = "$" + target.towerBlueprint.GetSellValue();

        rangeIndicator.transform.localScale = new Vector3(target.getTurretRange() * 2, 0, target.getTurretRange() * 2);

        uI.SetActive(true);
        rangeIndicator.SetActive(true);

    }

    public void Hide ()
    {
        uI.SetActive(false);
        rangeIndicator.SetActive(false);
    }

    public void Upgrade ()
    {
        target.UpgradeTurret();
        //closes upgrade/sell ui after upgrading
        BuildManager.instance.DeselectNode(); //change to not close after upgrading
    }

    public void Sell ()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
