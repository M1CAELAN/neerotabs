using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool is_pause = false;
    public GameObject pause_menu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (is_pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pause_menu.SetActive(true);
        Time.timeScale = 0.0f;
        is_pause = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pause_menu.SetActive(false);
        Time.timeScale = 1f;
        is_pause = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene("menu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
