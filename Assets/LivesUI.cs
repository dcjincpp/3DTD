using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{

    public TMPro.TextMeshProUGUI livesText;

    // Update is called once per frame
    void Update()
    {
        livesText.text = PlayerResources.Lives.ToString() + " Lives";
    }
}
