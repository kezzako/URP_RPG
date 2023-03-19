using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    public void OnPause(InputValue value)
    {
        if (gameIsPaused)
        {
            resume();
        }
        else
        {
            pause();
        }
    }

    public void pause()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
        DisableControl();
        pauseMenuUI.SetActive(true);
    }

    public void resume()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        EnableControl();
        pauseMenuUI.SetActive(false);

    }

    public void loadSettings()
    {


    }

    public void quitGame()
    {
        Application.Quit();
    }

    void DisableControl()
    {
        Debug.Log("DisableControl");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log(player);
        player.GetComponent<PlayerController>().enabled = false;

    }

    void EnableControl()
    {
        Debug.Log("EnableControl");

        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().enabled = true;
    }


}
