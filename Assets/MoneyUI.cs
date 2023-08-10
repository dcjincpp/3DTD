using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI moneyText;
    
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + PlayerResources.Money.ToString();
    }
}
