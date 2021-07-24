using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool isPaused;

    public GameObject pauseMenuObject;
    public GameObject ingameOverlay;

    private void Awake()
    {
        isPaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            Pause(isPaused);
        }

        // TODO: JUST FOR TESTING: RELOADING THE SCENE, REMOVE FROM FINAL BUILD
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause(bool isPaused)
    {
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = isPaused ? 0f : 1f;
        ingameOverlay.SetActive(!isPaused);
        pauseMenuObject.SetActive(isPaused);
    }
}
