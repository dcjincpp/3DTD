using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool GameEnded;
    
    public GameObject gameOverUI;

    void Start()
    {
        GameEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEnded)
        {
            return;
        }

        if (PlayerResources.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame ()
    {
        GameEnded = true;
        gameOverUI.SetActive(true);
    }
}
