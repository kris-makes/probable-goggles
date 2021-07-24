using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Access game manager script
    GameManager gameManager;

    // Menu pages
    public GameObject settings;
    public GameObject mainMenu;
    public GameObject quit;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    // Resume the game
    public void Resume()
    {
        GameManager.isPaused = false;
        gameManager.Pause(false);
    }

    // Show settings part
    public void ShowSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
        quit.SetActive(false);
    }

    // Show the part about the main menu
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        quit.SetActive(false);
    }

    // Show quitting part of the menu
    public void ShowQuit()
    {
        quit.SetActive(true);
        settings.SetActive(false);
        mainMenu.SetActive(false);
    }

    void HideMenus()
    {
        settings.SetActive(false);
        mainMenu.SetActive(false);
        quit.SetActive(false);
    }

    // Go to main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        // TODO: Valamilyen mentés ide?
    }

    // Quit the game
    public void Quit()
    {
        // TODO: Valamilyen mentés ide?
        Application.Quit();
    }
}
