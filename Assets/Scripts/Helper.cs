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


    public PauseMenu FindPauseMenuInScene() {
        GameObject pause = GameObject.Find("PauseMenuCanvas");
        return pause.GetComponent<PauseMenu>();
    }


    public StatusBar FindHealthBarInScene() {
        GameObject healthBar = GameObject.Find("HealthBar");
        return healthBar.GetComponent<StatusBar>();
    }


    public Health FindPlayerHealthInScene() {
        GameObject playerHealth = GameObject.FindWithTag("Player");
        return playerHealth.GetComponent<Health>();
    }

    public DayController GetDayControllerInScene() {
        GameObject dayController = GameObject.FindGameObjectWithTag("DayController");
        return dayController.GetComponent<DayController>();
    }

    public HighScore FindHighScoreInScene() {
        GameObject highScoreUI = GameObject.Find("HighScore");
        return highScoreUI.GetComponent<HighScore>();
    }
}
