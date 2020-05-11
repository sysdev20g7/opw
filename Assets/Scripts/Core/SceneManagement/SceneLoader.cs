using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The SceneLoader is responsible for loading and unloading scenes.
/// It is called upon exit and enter of a scene
/// It also adds a nice fadeout animation during the scene transition.
/// </summary>
public class SceneLoader : MonoBehaviour {
    private ObjectController objectcontroller;
    private static bool DEBUG_SCENEMGMT = true;
    private int _currentSceneIndex;
    private int _requestedSceneIndex;
    
    public Animator sceneAnimation;
    // Update is called once per frame
    public float animationDuration = 1f;
    public static int MAX_NUM_SCENES = 3; // Equals the number of valid scenes;

    void Update() {
        if (Input.GetKeyDown(KeyCode.PageUp)) {
            if (DEBUG_SCENEMGMT) PrintDebug("PageUp");
            // Write data before switching scene
            LoadNextScene(true);
        }
        else if (Input.GetKeyDown(KeyCode.PageDown)) {
            // Write data before switching scene
            if (DEBUG_SCENEMGMT) PrintDebug("PageDown");
            LoadNextScene(false);
        }
    }

    /// <summary>
    /// Called when the instance is loaded
    /// </summary>
    void Start() {
        this.sceneAnimation = GameObject.Find("BlackFade").GetComponent<Animator>();
        // Updates scene index to current scene before invoking switch
       this._currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Load last saved player pos for this scene
        this.objectcontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectController>();
        if (objectcontroller == null) {
            
        } else {
            if (!objectcontroller.PlayerHasVisitedScene(this._currentSceneIndex)) {
                GameObject g = GameObject.Find("PlayerSpawn");
                objectcontroller.SetPlayerPos(g.transform.position, this._currentSceneIndex);
                objectcontroller.SetPlayerVisitedScene(this._currentSceneIndex, true);
            }
            objectcontroller.lastInGameScene = this._currentSceneIndex;
            objectcontroller.runningInGame = true;
            objectcontroller.LoadPlayerData(this._currentSceneIndex);
            objectcontroller.LoadEnemyPosInScene(this._currentSceneIndex);
            if (objectcontroller.UpdateBackgroundMusic()) {
                Debug.Log("Sceneloader successfully set volume to " + objectcontroller.musicVolume);
            }
        }
    }


    /// <summary>
    /// Returns the current scene as an int
    /// </summary>
    /// <returns>current scene index</returns>
    public int GetCurrentScene() {
       return SceneManager.GetActiveScene().buildIndex;
    }
    /*
     * The LoadNextScene loads the next or prevous scene, seen from the index of the current scene.
     * It's important to note that the currentSceneIndex is updated before switching,
     * becuase the SceneManager instance dies when switcing scene.
     * @next true == loads the next scene / false == loads the prev scene
     */
    public void LoadNextScene(bool next) {
        PrintDebug("LoadNextScene:" + next);
        if (next) {
            this._requestedSceneIndex = this._currentSceneIndex + 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" 
                                             + this._currentSceneIndex + ";NEXT:"
                                             + this._requestedSceneIndex);
            StartCoroutine(LoadScene(this._requestedSceneIndex,true));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:"
                                                            + this._currentSceneIndex);
        }
        else {
            this._requestedSceneIndex = this._currentSceneIndex - 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" + this._currentSceneIndex + ";NEXT:" + this._requestedSceneIndex);
            StartCoroutine(LoadScene(this._requestedSceneIndex,true));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:" + this._currentSceneIndex);
        }
    }

    /// <summary>
    ///  This function starts transition animation and loads
    ///  the selected scene
    /// </summary>
    /// <param name="scene">Scene index to load</param>
    /// <param name="saveCurrentPos"> set this to true if you want
    /// to save player and enemy pos. In menus this is preferred to be false,
    /// in game this is preferred to be true </param> 
    public void LoadSpecifedScene(int scene, bool saveCurrentPos) {
        StartCoroutine(LoadScene(scene,saveCurrentPos));
    }
    
    /// <summary>
    /// This function can be used to print debug messages with a prefix to differentiate the
    /// messages from other debug messages.
    /// </summary>
    /// <param name="msg">Message to be printed</param>
    private void PrintDebug(string msg) {
        Debug.Log("DEBUG-SCENEMGT:" + msg);
    }

    /// <summary>
    /// Switches the scene to a specified scene index. It also saves the
    /// the positions of the player and enemies in memory if requested
    /// </summary>
    /// <param name="sceneIndex">Scene index to switch to</param>
    /// <param name="savePositions">boolean true/false for saving of pos</param>
    /// <returns></returns>
    // savePositions is not needed and should be refactored
    IEnumerator LoadScene(int sceneIndex,bool savePositions) {
        if (sceneIndex <= MAX_NUM_SCENES && sceneIndex > -1) {
            Debug.Log("Switched from scene " + this._currentSceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(this._currentSceneIndex).name
                      + ")");

            
            sceneAnimation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);    // Break and sleep 1 sec
            if (savePositions) { // <- remove this if
                objectcontroller.WritePlayerData(this._currentSceneIndex); 
                objectcontroller.WriteEnemyPosInScene(this._currentSceneIndex);
            }
            SceneManager.LoadScene(sceneIndex);    // Run again to fade out 

            Debug.Log("Switched to scene " + sceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(sceneIndex).name + ")");
        }
        else {
            PrintDebug("ENUM-INVALID-INDEX: The scene \"" + sceneIndex
                                                          + "\" requested is out of bounds. ");
        }
    }
}