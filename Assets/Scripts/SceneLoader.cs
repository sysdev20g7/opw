using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public Animator animation;

    public float animationDuration = 1f;
    // Update is called once per frame

    public static int MAX_NUM_SCENES = 2; // Equals the number of valid scenes;
                                          // see File -> Build settings in Unity...
    private static bool DEBUG_SCENEMGMT = false;
    private static int START_SCENE = 0;
    private bool _initalized= false;
    private int _currentSceneIndex;
    private int _requestedSceneIndex;
    private float[] _currentPosition; 

    void Awake() {
        if (this._initalized == false) {
            PrintDebug("Initializing SceneManager");
            this._initalized = true;
            this._currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //Set initial index of current scene
            if (DEBUG_SCENEMGMT) PrintDebug("Started SceneManager;index="
                                            + this._currentSceneIndex);
            if (this._requestedSceneIndex != this._currentSceneIndex) {
                this._requestedSceneIndex = START_SCENE;
                if (DEBUG_SCENEMGMT) PrintDebug("Updated to currentScene;index=" + this._currentSceneIndex);
                StartCoroutine(LoadScene(this._requestedSceneIndex));
                this._currentSceneIndex = this._requestedSceneIndex;
            }

        }
        if (DEBUG_SCENEMGMT) PrintDebug("Woke up SceneManager");
        
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PrintDebug("LeftClick");
            LoadNextScene(true);
        }
        else if (Input.GetMouseButtonDown(1)) {
            PrintDebug("RightClick");
            LoadNextScene(false);
        }
    }

    /*
     * The LoadNextScene loads the next or prevous scene, seen from the index of the current scene.
     * It's important to note that the currentSceneIndex is updated before switching
     * The build index order can be set under tile File -> Build Settings... menu
     * @next true == loads the next scene / false == loads the prev scene
     */
    public void LoadNextScene(bool next) {
        PrintDebug("LoadNextScene:" + next);
         // Updates scene index to current scene before invoking switch
        this._currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (next) {
            this._requestedSceneIndex = this._currentSceneIndex + 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" 
                                             + this._currentSceneIndex + ";NEXT:"
                                             + this._requestedSceneIndex);
            StartCoroutine(LoadScene(this._requestedSceneIndex));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:"
                                                            + this._currentSceneIndex);
        }
        else {
            this._requestedSceneIndex = this._currentSceneIndex - 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" + this._currentSceneIndex + ";NEXT:" + this._requestedSceneIndex);
            StartCoroutine(LoadScene(this._requestedSceneIndex));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:" + this._currentSceneIndex);
        }
    }

    /*
     *  This function can be used to print debug messages with a prefix to differentiate the
     *  messages from other debug messages.
     *  @param msg - The message to be printed
     */
    private void PrintDebug(string msg) {
        Debug.Log("DEBUG-SCENEMGT:" + msg);
    }

    /* This function loads a requested scene, as long as the scene requested has a valid
     index value. If the index value is not valid, the SceneManager will not load the scene.*/
    IEnumerator LoadScene(int sceneIndex) {
        if (sceneIndex <= MAX_NUM_SCENES && sceneIndex > -1) {
            Debug.Log("Switched from scene " + this._currentSceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(this._currentSceneIndex).name
                      + ")");

            animation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndex);

            //this._currentSceneIndex = sceneIndex;
            Debug.Log("Switched to scene " + sceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(sceneIndex).name + ")");
        }
        else {
            PrintDebug("ENUM-INVALID-INDEX: The scene \"" + sceneIndex
                                                          + "\" requested is out of bounds. ");
        }

        /* This print will veryfy that Unity does not respect writing to global variables
         * in a subroutine. As CurrentScene can not be updated from here, it must be
         * updated from the method that requests the scene transition.. See line 44 for an example
         */
        //if (DEBUG_SCENEMGMT) PrintDebug("ENUM-RETURN-CURRENTINDEX:" + this._currentSceneIndex);
    }
}