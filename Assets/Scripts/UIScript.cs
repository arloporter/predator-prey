using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    void Start()
    {
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;

    }

    public void QuitGame()
    {
        // This Will not quit in editor, but will quit in a proper build of the game
        print("Quit Game Successful");
        Application.Quit();
    }
}
