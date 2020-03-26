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
    private ObjectController obj;
    private SaveGame checkSave;

    // Constructor
    public MainMenu() {
        this._helper = new Helper(); //create helper
        
    }
    
    void Start() {
        // Check if save exists, and set load button accordingly
        obj = this._helper.FindObjectControllerInScene();
        checkSave = new SaveGame();
        EnableLoadButton(checkSave.SaveExists(JSON));
    }

    public void PlayGame()
    {
        // Check if a save exist, ask user to continue before delete?
        PromptExistingGame(checkSave.SaveExists(JSON));
    }



    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }


    /// <summary>
    ///  This function loads a saved game
    /// </summary>
    public void LoadGame() {
        if (checkSave.SaveExists(JSON)) {
            obj.LoadGame();
        }
    }

    /// <summary>
    ///  This function deletes exisitng data, including save file on disk,
    /// and loads a new game from the first scene
    /// </summary>
    public void DeleteGame() {
        Debug.Log("New game; deleted old data and save");
        obj.ResetGame(); // Delete data, and create new data slots 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    /// <summary>
    ///  This function checks if a save is present, and asks
    /// the user if we shall delete the existing save, before
    /// starting a new game. Can this be a GUI popup maybe ? 
    /// </summary>
    /// <param name="existingGame"></param>
    private void PromptExistingGame(bool gameExists) {
        if (gameExists) {
            // A game save exists, shall we delete ?
            
            /// Prompt the user when clicking new game,
            /// to confirm if he want to really proceed
            /// with deleting the game save.
            ///
            if (true) {
                // The user said yes, we'll delete the data
                DeleteGame(); 
            } 
        }
        else {
            // No game exits, reset data anyway
            DeleteGame();
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
                loadBtn.SetActive(true);    // Enable the button
            }
            else {
                Debug.Log("Save not found, load disabled");
                loadBtn.GetComponent<Button>().interactable = false;
                loadBtn.GetComponent<Button>().transition = Selectable.Transition.None;
                loadBtn.GetComponent<Animator>().enabled = false;
                loadBtn.SetActive(false); // Disable the button
                
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
