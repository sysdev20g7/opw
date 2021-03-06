﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The MainMenu class is responsible for providing interactive
/// methods for UI-elements in the Menu scenes, 
/// </summary>
public class MainMenu : MonoBehaviour {
    private static string _loadBtnTag = "MenuButtonLoadGame";
    private Helper _helper;
    private ObjectController obj;
    private SaveGame checkSave;
    private readonly bool INGAME_DEBUG = true;
    public int SCN_OPTIONS_MENU = 4;
    public int SCN_MAIN_MENU = 0;

    // Constructor
    public MainMenu() {
        this._helper = new Helper(); //create helper
        
    }
    
    void Start() {
        // Check if save exists, and set load button accordingly
        obj = this._helper.FindObjectControllerInScene();
        checkSave = new SaveGame();
        EnableLoadButton(checkSave.SaveExists(SaveType.Json));
        obj.UpdateBackgroundMusic(); //update vol when returning to meny
        UpdateMusicSlider(obj.musicVolume);
    }

    public void PlayGame()
    {
        // Check if a save exist, ask user to continue before delete?
        bool existing = checkSave.SaveExists(SaveType.Json);
        if (existing) {
            if (INGAME_DEBUG == true) Debug.Log("MainMenu:Started_new_game");
        }
        else {
            
            if (INGAME_DEBUG == true) Debug.Log("MainMenu:Save_exsists,_prompting");
        }
        PromptExistingGame(checkSave.SaveExists(SaveType.Json));
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
        if (checkSave.SaveExists(SaveType.Json)) {
            if (INGAME_DEBUG == true) Debug.Log("MainMenu:Loaded_exsisting_game");
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
    /// Enable a UI popup confirmation to make the user confirm that he
    /// wants to create a new game (and overwrite the old one)
    /// </summary>
    /// <param name="enabled"></param>
    public void EnablePopupCanvas(bool enabled) {
        try {
            GameObject Popup = GameObject.Find("NewGameCanvas");
            GameObject Canvas = GameObject.Find("Canvas");
            GameObject Menu = Canvas.transform.Find("MainMenu").gameObject;
            if (enabled) {
                Debug.Log("Found canvas");
                Popup.GetComponent<Canvas>().enabled = true;
                Menu.SetActive(false);
            }
            else {
                Popup.GetComponent<Canvas>().enabled = false;
                Menu.SetActive(true);
            }
        }
        catch (Exception e) {
            Debug.Log("Error occured when trying to display popup");
            Debug.Log("Most likely you've forgotten to enable the canvas" +
                      "game object, or the menu game object");
            throw;
        }
    }

    /// <summary>
    /// Used by the Back-button UI-element in the OptionsMenu
    /// to decide either to return to MainMenu or go to PauseMenu
    /// </summary>
    /// <param name="enabled"></param>
    public void EnableOptionsMenu(bool enabled) {

        if (obj.lastInGameScene == null) {
            Debug.Log("From Main Menu");
            EnableMainMenuOptions(enabled);

        } else {
            Debug.Log("From Pause Menu");
            if (enabled == false) {
                ReturnToPauseMenu();
            }
        }
        
        // update toggle highscore if found in menu
        GameObject toggleObj = GameObject.Find("ToggleHS");
        if (toggleObj = GameObject.Find("ToggleHS")) {
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            toggle.isOn = obj.runningGame.keepHighScore;
        }
    }
    
    /// <summary>
    ///  Used In MainMenu -scene: Toggles the display of
    /// either the MainMenu canvas or OptionMenu canvas
    /// </summary>
    /// <param name="enableOptions"> State of Enable OptionMenu Canvas</param>
    public void EnableMainMenuOptions(bool enableOptions) {
        try {
            GameObject Canvas = GameObject.Find("Canvas");
            GameObject Options = Canvas.transform.Find("OptionsMenu").gameObject;
            GameObject Menu = Canvas.transform.Find("MainMenu").gameObject;
            
            if (enableOptions) {
                Options.SetActive(true);
                Menu.SetActive(false);
            }
            else {
                Options.SetActive(false);
                Menu.SetActive(true);
                
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            if (INGAME_DEBUG == true) Debug.Log("Exception: " + e);
            throw;
        }
    }



    /// <summary>
    ///  Flips the bool state of keep high score in
    /// runningGame GameData. Also update visual toggle
    /// indicator in UI
    /// </summary>
    public void EnableSaveHighScore() {
        Helper findObj = new Helper();
        ObjectController obj = findObj.FindObjectControllerInScene();
        
        if (obj is null) { return; } //return if fault
 
        // flip state of keep high score
        if (obj.runningGame.keepHighScore) {
            obj.runningGame.keepHighScore = false;
        }
        else {
            obj.runningGame.keepHighScore = true;
        }
        
        GameObject toggleObj = GameObject.Find("ToggleHS");
        if (!(toggleObj is null)) {
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            toggle.isOn = obj.runningGame.keepHighScore;
        }
        
    }
    
    /// <summary>
    /// Update the music value from the volume slider
    /// </summary>
    public void UpdateMusicValue() {
        GameObject volumeSlider = GameObject.Find("Slider");
        if (!(volumeSlider == null)) {
            obj.musicVolume = volumeSlider.GetComponent<Slider>().value;
            if (obj.UpdateBackgroundMusic()) {
                Debug.Log("Updated running music volume to " + obj.musicVolume);
            }
        }

    }

    /// <summary>
    /// The UpdateMusicSlider method updates the UI slider element
    /// to the specified volume value provided
    /// </summary>
    /// <param name="volume">representation of sound level</param>
    private void UpdateMusicSlider(float volume) {
        GameObject volumeSlider = GameObject.Find("Slider");
        if (!(volumeSlider == null)) {
            volumeSlider.GetComponent<Slider>().value = volume;
        }
    }

    /// <summary>
    /// This method is used when exiting the options menu
    /// when using the Back-button UI-element; to then if
    /// we should exit to Main Menu or Pause Menu.
    /// </summary>
    private void ReturnToPauseMenu() {
        Helper opMenuHelper = new Helper();
        ObjectController controller
            = opMenuHelper.FindObjectControllerInScene();
        
        if (controller.lastInGameScene is null) {
            // We are currently in main meny, because it's set to null
        } else {
            // We are currently in-game
            int returnScene = (int) controller.lastInGameScene;
            SceneManager.LoadScene(returnScene);
        }
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
            EnablePopupCanvas(true);
            
            // Prompt the user when clicking new game,
            // to confirm if he want to really proceed
            // with deleting the game save.
            
            if (true) {
                // The user said yes, we'll delete the data
                //DeleteGame();  //Deletion is done on Yes button on GUI
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
            }
            else {
                Debug.Log("Save not found, load disabled");
                loadBtn.GetComponent<Button>().interactable = false;
                loadBtn.GetComponent<Button>().transition = Selectable.Transition.Animation;
                // Set the load button to disabled state
                loadBtn.GetComponent<Animator>().SetTrigger("Disabled");
                loadBtn.GetComponent<Animator>().enabled = true;
                loadBtn.GetComponent<Button>().onClick = null;
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Debug.Log("Unable to find " + _loadBtnTag.ToString()
                      + " in this menu");
        }
    }
    
    /// <summary>
    /// This function is just to show an example of how to
    ///  save or the game with the global objectcontroller.
    ///
    /// It should not be called in this class, but rather be implemented
    /// as needed in the classes or menus that need to load/save the game.
    /// </summary>
    private void SaveGameLoadGameExample() {
        bool save = false, load = false;
        if (save) {
            Helper saveHelper = new Helper();
            ObjectController saveController 
                = saveHelper.FindObjectControllerInScene();
            // Does the real work of saving the game
            saveController.SaveGame();
        }

        if (load) {
            Helper loadHelper = new Helper();
            ObjectController loadController
                = loadHelper.FindObjectControllerInScene(); 
            // Does the real work of loading the game and switching scene
            loadController.LoadGame();
        }
    }
}
