using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *  The SceneLoader loads a new scene after requested to switch scene.
 * Usage: 1. Check that MAX_NUM_SCENES corresponds to # of scenes in the
 *           File -> Build Settings
 *        2. Add the SceneLoader -prefab to each scene that needs switching
 *        3. Check that Player Pos and Camera Pos is set properly on each scene
 *
 *  PS: The SceneLoader together with other Unity-objects will be destroyed
 *      between loading scenes. 
 */
public class SceneLoader : MonoBehaviour {
    private ObjectController objectcontroller;
    public Animator sceneAnimation;

    public float animationDuration = 1f;
    // Update is called once per frame

    public static int MAX_NUM_SCENES = 3; // Equals the number of valid scenes;
                                          // see File -> Build settings in Unity...
    private static bool DEBUG_SCENEMGMT = false;
    private bool firstScene = true;
    private int _currentSceneIndex;
    private int _requestedSceneIndex;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (DEBUG_SCENEMGMT) PrintDebug("LeftClick");
            // Write data before switching scene
            LoadNextScene(true);
        }
        else if (Input.GetMouseButtonDown(1)) {
            // Write data before switching scene
            if (DEBUG_SCENEMGMT) PrintDebug("RightClick");
            LoadNextScene(false);
        }
    }

    void Start() {
        this.sceneAnimation = GameObject.Find("BlackFade").GetComponent<Animator>();
        // Updates scene index to current scene before invoking switch
       this._currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Load last saved player pos for this scene
        this.objectcontroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectController>();
        if (objectcontroller == null) {
            
        } else {
            objectcontroller.LoadSavedPlayerPos(this._currentSceneIndex);
            objectcontroller.LoadEnemyPosInScene(this._currentSceneIndex);
        }
    }

    void Awake() {
    }

    void OnDestroy() {
        
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

    /*
     *  This function can be used to print debug messages with a prefix to differentiate the
     *  messages from other debug messages.
     *  @param msg - The message to be printed
     */
    private void PrintDebug(string msg) {
        Debug.Log("DEBUG-SCENEMGT:" + msg);
    }

    /* This function  switches the scene. It runs twice to do the switch
     * First it checks that requested scene is valid, if true then:
     * 1. Trigger - switch state to animation BlackFade(End) fading to black
     * 2. Break execution and sleep for one secound 
     * 3. Call SceneManager.LoadScene to switch scene(destroy current, create new)
     * 4. Run itself again
     * 5. Trigger switch state to animation BlackFade(Begin) fade to transparent
     * 5. The scene is now visible
     * If scene is not valid, the SceneManager will not load the scene.
     */
    IEnumerator LoadScene(int sceneIndex,bool savePositions) {
        if (sceneIndex <= MAX_NUM_SCENES && sceneIndex > -1) {
            Debug.Log("Switched from scene " + this._currentSceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(this._currentSceneIndex).name
                      + ")");

            
            sceneAnimation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);    // Break and sleep 1 sec
            if (savePositions) {
            objectcontroller.WriteSavedPlayerPos(this._currentSceneIndex); 
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