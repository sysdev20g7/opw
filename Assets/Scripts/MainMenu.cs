using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private static string _loadBtnTag = "MenuButtonLoadGame";
    private static int JSON = 1, BINARY = 2;
    private Helper _helper;

    // Constructor
    public MainMenu() {
        this._helper = new Helper(); //create helper
        
    }

    public void PlayGame()
    {
        ObjectController obj = this._helper.FindObjectControllerInScene();
        // Reset existing game data, delete existing saves
        Debug.Log("New game; deleted old data and save");
        obj.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    void Start() {
        // Check if save exists, and set load button accordingly
        SaveGame checkSave = new SaveGame();
        EnableLoadButton(checkSave.SaveExists(JSON));
    }

    /// <summary>
    ///  This function loads a saved game
    /// </summary>
    public void LoadGame() {
        ObjectController obj = this._helper.FindObjectControllerInScene();
        SaveGame checkSave = new SaveGame();
        if (checkSave.SaveExists(JSON)) {
            obj.LoadGame();
        }
    }

    /// <summary>
    /// This function disables the load game button 
    /// if enabled is set to false
    /// </summary>
    /// <param name="enabled">true to enable the button, otherwise false</param>
    private void EnableLoadButton(bool enabled) {
        try {
            GameObject loadBtn = GameObject.FindGameObjectWithTag(_loadBtnTag);
            if (enabled) {
                Debug.Log("Save exists, load enabled");
                loadBtn.GetComponent<Button>().transition = Selectable.Transition.Animation;
                loadBtn.GetComponent<Button>().interactable = true;
                loadBtn.GetComponent<Animator>().enabled = true;
            }
            else {
                Debug.Log("Save not found, load disabled");
                loadBtn.GetComponent<Button>().interactable = false;
                loadBtn.GetComponent<Button>().transition = Selectable.Transition.None;
                loadBtn.GetComponent<Animator>().enabled = false;
                
                /// FIXME
                /// Insert other related things to to here
                /// to indicate that it's not possible to
                /// load a save ( for example add a grey overlay, etc.)

            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Debug.Log("Unable to find " + _loadBtnTag.ToString()
                      + "in this menu");
        }
    }

}
