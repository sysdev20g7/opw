using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Helper {

    /// <summary>
    ///  This function finds returns the ObjectController in the Scene
    /// </summary>
    /// <returns></returns>
    public ObjectController FindObjectControllerInScene() {
        ObjectController ob 
            = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectController>();
        return ob;
    }

    
    /// <summary>
    ///  Finds the SceneLoader object in the current running Scene
    /// </summary>
    /// <returns></returns>
    public SceneLoader FindSceneLoaderInScene() {
        GameObject loader = GameObject.Find("SceneLoader");
        return loader.GetComponent<SceneLoader>();
    }
}
