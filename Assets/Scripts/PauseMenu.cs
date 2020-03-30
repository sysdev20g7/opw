using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public bool gameIsPaused = false; //removed static as option meny uses this
    public GameObject pauseMenuUI;
    private string MainMenu = "Main Menu";
    private string OptionsMenu = "Options Menu";
    private ObjectController pauseController;
    private int _currentScene;

    public PauseMenu() {
    }
    void Start() {
        Helper pauseHelper = new Helper();
        this.pauseController
            = pauseHelper.FindObjectControllerInScene();
        this.gameIsPaused = !pauseController.runningInGame;
        this._currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        pauseController.runningInGame = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        pauseController.runningInGame = false;
    }

    public void LoadOptions() {
        pauseController.lastInGameScene = _currentScene;
        pauseController.WriteSavedPlayerPos(_currentScene);
        SceneManager.LoadScene(OptionsMenu);
        Debug.Log("Loading options...");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        pauseController.WriteSavedPlayerPos(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(MainMenu);
        Debug.Log("Loading main menu...");
    }

    public void SaveGameButton() {
            // Does the real work of saving the game
            pauseController.WriteSavedPlayerPos(SceneManager.GetActiveScene().buildIndex);
            pauseController.SaveGame();
    }
}
