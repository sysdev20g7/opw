using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    /// <summary>
    /// Contains UI-functions and logic for PauseMenu
    /// </summary>
    public bool gameIsPaused = false; //removed static as option meny uses this
    public GameObject pauseMenuUI;
    private string MainMenu = "Main Menu";
    private string OptionsMenu = "Options Menu";
    private ObjectController pauseController;
    private int _currentScene;

    void Start() {
        Helper pauseHelper = new Helper();
        this.pauseController
            = pauseHelper.FindObjectControllerInScene();
        this.gameIsPaused = !pauseController.runningInGame;
        this._currentScene = SceneManager.GetActiveScene().buildIndex;
        Resume();
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

    /// <summary>
    /// Used by UI-button to open the options menu 
    /// </summary>
    public void LoadOptions() {
        pauseController.lastInGameScene = _currentScene;
        pauseController.WritePlayerData(_currentScene);
        SceneManager.LoadScene(OptionsMenu);
        Debug.Log("Loading options...");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        pauseController.WritePlayerData(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(MainMenu);
        Debug.Log("Loading main menu...");
    }

    /// <summary>
    /// Used by UI-button to save the game
    /// </summary>
    public void SaveGameButton() {
            // Does the real work of saving the game
            pauseController.WritePlayerData(SceneManager.GetActiveScene().buildIndex);
            pauseController.SaveGame();
    }



        
    /// <summary>
    /// Displays a indicator for visually giving the user
    /// feedback if the save was succsessful
    /// </summary>
    /// <param name="yes">Set to true to display the OK indicator</param>
    public void DisplaySuccessfulSave(bool yes) {
        try {
            GameObject Canvas = GameObject.Find("PauseMenu");
            GameObject SaveIcon = Canvas.transform.Find("SavedOk").gameObject;

            if (yes) {
                Debug.Log("Saved OK");
                SaveIcon.SetActive(true);
                StartCoroutine(HideGameObject(2, SaveIcon));
            }
            
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Co-routine for hiding SavedOK indicator
    /// </summary>
    /// <param name="delay">off-delay</param>
    /// <param name="gb">the GameObject to hide</param>
    /// <returns></returns>
    IEnumerator HideGameObject(float delay, GameObject gb) {
        Debug.Log("Fired Corotine");
        yield return new WaitForSecondsRealtime(delay);
        
        Debug.Log("Hidden Save OK");

        if (!(gb is null)) {
            gb.SetActive(false);
        }
    }
}
