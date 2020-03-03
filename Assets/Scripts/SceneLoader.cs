using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator animation;

    public float animationDuration = 1f; 
    // Update is called once per frame

    public static int MAX_NUM_SCENES = 2; // Equals the number of valid scenes; see File -> Build settings in Unity...
    private int currentSceneIndex;
    private int requestedSceneIndex;

    void start() {
        PrintDebug("Started SceneManager;index=" + currentSceneIndex);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PrintDebug("Updated to currentScene;index=" + currentSceneIndex);
    }
    void Update() {
        PrintDebug("Updated to currentScene;index=" + currentSceneIndex);
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
    * The LoadNextScene loads the next or prevous scene, seen from the  index of the current scene.
    * The build index order can be set under tile File -> Build Settings... menu
    * @next true == loads the next scene / false == loads the prev scene
    */
    public void LoadNextScene( bool next) {
        PrintDebug("LoadnextScene:" + next);
        if (next) {
            requestedSceneIndex = currentSceneIndex + 1;
            PrintDebug("Coroutine Start" + "CURR:" + currentSceneIndex + ";NEXT:" + requestedSceneIndex);
            StartCoroutine(LoadScene(requestedSceneIndex));
            PrintDebug("Coroutine End" + "CURR:" + currentSceneIndex);
        } else {
            requestedSceneIndex = currentSceneIndex - 1;
            PrintDebug("Coroutine Start" + "CURR:" + currentSceneIndex + ";NEXT:" + requestedSceneIndex);
            StartCoroutine(LoadScene(requestedSceneIndex));
            PrintDebug("Coroutine End" + "CURR:" + currentSceneIndex);
        }
    }

    private void PrintDebug(string msg) {
        Debug.Log("DEBUG-SCENEMGT: CURR" + currentSceneIndex + "MSG:"+ msg);
        
    }
    /* This function loads a requested scene, as long as the scene requested has a valid
     index value. If the index value is not valid, the SceneManager will not load the scene.*/
    IEnumerator LoadScene(int sceneIndex) {
        PrintDebug("ENUM-SCENEINDEX:" + sceneIndex);
        if (sceneIndex <= MAX_NUM_SCENES && sceneIndex > -1) {
            animation.SetTrigger("Begin");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndex);
            PrintDebug("ENUM-LOADED:" + sceneIndex + "CURRENTSCENE IS:" + currentSceneIndex);
        Debug.Log("ENUM-SWITCHED-TO"
                  + currentSceneIndex + ": " + SceneManager.GetSceneByBuildIndex(currentSceneIndex).name);
        } else {
            Debug.Log("ENUM-INVALID-INDEX:\"" + currentSceneIndex + "\"");
        }
        currentSceneIndex = sceneIndex;
        PrintDebug("ENUM-RETURN-CURRENTINDEX:" + currentSceneIndex);
    }
}
