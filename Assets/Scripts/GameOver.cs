using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roundsText;

    // Start is called before the first frame update
    void OnEnable ()
    {
        roundsText.text = PlayerResources.Rounds.ToString();
    }

    // Update is called once per frame
    public void Retry ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu ()
    {
        Debug.Log("Go to menu");
    }
}
