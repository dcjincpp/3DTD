using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roundsText;
    public SceneFader sceneFader;
    public string menuSceneName = "MainMenu";

    // Start is called before the first frame update
    void OnEnable ()
    {
        roundsText.text = PlayerResources.Rounds.ToString();
    }

    // Update is called once per frame
    public void Retry ()
    {
        sceneFader.FadeOutTo(SceneManager.GetActiveScene().name);
    }

    public void Menu ()
    {
        sceneFader.FadeOutTo(menuSceneName);
    }
}
