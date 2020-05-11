using UnityEngine;
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


    /// <summary>
    /// Find the PauseMenu object in the scene, and return it
    /// </summary>
    /// <returns>Object of type PauseMenu</returns>
    public PauseMenu FindPauseMenuInScene() {
        GameObject pause = GameObject.Find("PauseMenuCanvas");
        return pause.GetComponent<PauseMenu>();
    }


    /// <summary>
    /// Find the HealthBar object in the scene, and return it
    /// </summary>
    /// <returns>Object of type HealthBar</returns>
    public StatusBar FindHealthBarInScene() {
        GameObject healthBar = GameObject.Find("HealthBar");
        return healthBar.GetComponent<StatusBar>();
    }


    /// <summary>
    /// Find the Player object in the scene, and return Health
    /// script component
    /// </summary>
    /// <returns>Object of type component HealthScript</returns>
    public Health FindPlayerHealthInScene() {
        GameObject playerHealth = GameObject.FindWithTag("Player");
        return playerHealth.GetComponent<Health>();
    }

    /// <summary>
    /// Find the DayController object in the scene, and return it
    /// </summary>
    /// <returns>Object of type DayController</returns>
    public DayController GetDayControllerInScene() {
        GameObject dayController = GameObject.FindGameObjectWithTag("DayController");
        return dayController.GetComponent<DayController>();
    }

    /// <summary>
    /// Find the HighScore object in the scene, and return it
    /// </summary>
    /// <returns>Object of type HighScore</returns>
    public HighScore FindHighScoreInScene() {
        GameObject highScoreUI = GameObject.Find("HighScore");
        return highScoreUI.GetComponent<HighScore>();
    }
}
