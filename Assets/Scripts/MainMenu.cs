using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "Game";
    public SceneFader sceneFader;

    public void Play ()
    {
        sceneFader.FadeOutTo(levelToLoad);
    }

    public void Quit ()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
