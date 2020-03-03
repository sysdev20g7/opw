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
    private int currentSceneIndex;
    private int requestedSceneIndex;

    void start() {
        if (DEBUG_SCENEMGMT) PrintDebug("Started SceneManager;index=" + currentSceneIndex);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //Set initial index of current scene
        if (DEBUG_SCENEMGMT) PrintDebug("Updated to currentScene;index=" + currentSceneIndex);
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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //Updates the scene index to current scene
        if (next) {
            requestedSceneIndex = currentSceneIndex + 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" 
                                             + currentSceneIndex + ";NEXT:"
                                             + requestedSceneIndex);
            StartCoroutine(LoadScene(requestedSceneIndex));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:"
                                                            + currentSceneIndex);
        }
        else {
            requestedSceneIndex = currentSceneIndex - 1;
            if (DEBUG_SCENEMGMT)
                PrintDebug("Coroutine Start" + "CURR:" + currentSceneIndex + ";NEXT:" + requestedSceneIndex);
            StartCoroutine(LoadScene(requestedSceneIndex));
            if (DEBUG_SCENEMGMT) PrintDebug("Coroutine End" + "CURR:" + currentSceneIndex);
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
            Debug.Log("Switched from scene " + currentSceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(currentSceneIndex).name
                      + ")");

            animation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndex);

            //currentSceneIndex = sceneIndex;
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
        //if (DEBUG_SCENEMGMT) PrintDebug("ENUM-RETURN-CURRENTINDEX:" + currentSceneIndex);
    }
}