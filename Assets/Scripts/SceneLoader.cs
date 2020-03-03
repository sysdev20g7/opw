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
    public Animator animation;

    public float animationDuration = 1f;
    // Update is called once per frame

    public static int MAX_NUM_SCENES = 2; // Equals the number of valid scenes;
                                          // see File -> Build settings in Unity...
    private static bool DEBUG_SCENEMGMT = false;
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

    /*
     * The LoadNextScene loads the next or prevous scene, seen from the index of the current scene.
     * It's important to note that the currentSceneIndex is updated before switching,
     * becuase the SceneManager instance dies when switcing scene.
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
    IEnumerator LoadScene(int sceneIndex) {
        if (sceneIndex <= MAX_NUM_SCENES && sceneIndex > -1) {
            Debug.Log("BØØ!");
            Debug.Log("Switched from scene " + this._currentSceneIndex + " ("
                      + SceneManager.GetSceneByBuildIndex(this._currentSceneIndex).name
                      + ")");

            animation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);    // Break and sleep 1 sec
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