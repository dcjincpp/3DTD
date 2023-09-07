using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject uI;

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu ()
    {
        TogglePause();
        SceneManager.LoadScene("MainMenu");
    }
}
