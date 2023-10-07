using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject uI;
    public SceneFader sceneFader;
    public string menuSceneName = "MainMenu";

    // void Update ()
    // {
    //     if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
    //     {
    //         TogglePause();
    //     }
    // }

    public void TogglePause ()
    {
        uI.SetActive(!uI.activeSelf);

        if(uI.activeSelf)
        {
            Time.timeScale = 0f;
            //Time.fixedDeltaTime when changing timescale to something other than 1 or 0
        } else {
            Time.timeScale = 1f;
        }
    }

    public void Retry ()
    {
        TogglePause();
        sceneFader.FadeOutTo(SceneManager.GetActiveScene().name);
    }

    public void Menu ()
    {
        TogglePause();
        sceneFader.FadeOutTo(menuSceneName);
    }
}
