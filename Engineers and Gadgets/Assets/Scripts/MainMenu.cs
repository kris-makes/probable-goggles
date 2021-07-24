using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Menu screens
    public GameObject startingScreen;
    public GameObject play;
    public GameObject multiplayer;
    public GameObject settings;
    public GameObject credits;

    // Other gameobjects
    public GameObject continueObject;
    public GameObject newGame;
    public GameObject howToPlay;
    public GameObject joinRoom;
    public GameObject makeRoom;
    public GameObject howToPlayMultiplayer;
    public GameObject generalSettings;
    public GameObject audioSettings;
    public GameObject graphicsSettings;


    // Private variables
    int numberOfMenuScreens;
    GameObject menuScreen;
    GameObject activeMenuScreen;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        startingScreen.SetActive(true);
        numberOfMenuScreens = transform.childCount;
        for (int i = 0; i < numberOfMenuScreens; ++i)
        {
            menuScreen = transform.GetChild(i).gameObject;
            if (!menuScreen.CompareTag("Important"))
                menuScreen.SetActive(false);
        }
    }

    // Play the game
    public void Play()
    {
        play.SetActive(true);
        activeMenuScreen = play;
        ShowContinueGame();
    }

    // Play with others
    public void Multiplayer()
    {
        multiplayer.SetActive(true);
        activeMenuScreen = multiplayer;
        ShowJoinRoom();
    }

    // Adjust some settings
    public void Settings()
    {
        settings.SetActive(true);
        activeMenuScreen = settings;
        ShowGeneralSettings();
    }

    // View the credits
    public void Credits()
    {
        credits.SetActive(true);
        activeMenuScreen = credits;
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit();
    }

    // Step back from a menu screen
    public void Back(GameObject backFrom)
    {
        startingScreen.SetActive(true);
        backFrom.SetActive(false);
    }

    // Show information about ability to continue from a started game
    public void ShowContinueGame()
    {
        continueObject.SetActive(true);
        newGame.SetActive(false);
        howToPlay.SetActive(false);
    }

    // Show information about ability to start a new game
    public void ShowNewGame()
    {
        newGame.SetActive(true);
        continueObject.SetActive(false);
        howToPlay.SetActive(false);
    }

    // Show information about how to play
    public void ShowHowToPlay()
    {
        howToPlay.SetActive(true);
        continueObject.SetActive(false);
        newGame.SetActive(false);
    }

    // Continue playing
    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Start a new game
    public void StartGame()
    {
        
    }

    // Show information about joining a multiplayer game
    public void ShowJoinRoom()
    {
        joinRoom.SetActive(true);
        makeRoom.SetActive(false);
        howToPlayMultiplayer.SetActive(false);
    }

    // Show information about making a multiplayer room
    public void ShowMakeRoom()
    {
        makeRoom.SetActive(true);
        joinRoom.SetActive(false);
        howToPlayMultiplayer.SetActive(false);
    }

    // Show information about how to play multiplayer;
    public void ShowHowToPlayMultiplayer()
    {
        howToPlayMultiplayer.SetActive(true);
        joinRoom.SetActive(false);
        makeRoom.SetActive(false);
    }

    // Join others in the game
    public void JoinRoom()
    {

    }

    // Make a multiplayer room
    public void MakeRoom()
    {

    }

    // Show general settings
    public void ShowGeneralSettings()
    {
        generalSettings.SetActive(true);
        audioSettings.SetActive(false);
        graphicsSettings.SetActive(false);
    }

    // Show audio settings
    public void ShowAudioSettings()
    {
        audioSettings.SetActive(true);
        generalSettings.SetActive(false);
        graphicsSettings.SetActive(false);
    }

    // Show graphics settings
    public void ShowGraphicsSettings()
    {
        graphicsSettings.SetActive(true);
        generalSettings.SetActive(false);
        audioSettings.SetActive(false);
    }
}
