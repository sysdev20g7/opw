using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = System.Object;

/// <summary>
///  The object controller is persistant (runs during the entire game) and
///  interact with different objects when needed, as e.g. handeling player 
///  position between scene switching.
///
/// TO TEST:
/// 1. Add ObjectController prefab to first scene in game
/// 2. Set ObjectController GameObject with GameController tag in meta
/// 3. Set Player in scene with Player-tag in meta (do this for each scene)
///
/// 4. Set prefix names for sprites to use when spawning npc
/// 5. Set enemy tag (used to select enemy game objects when storing data)
/// 6. Check that also the SceneLoader is added to each scene
/// Writing and recalling player pos works (per 04/03-20 ) 
/// </summary>
public class ObjectController : MonoBehaviour {

    public GameObject prefabZombie;
    public GameObject prefabPolice;
    public GameObject prefabPlayer;
    [ReadOnly]public int? lastInGameScene = null;
    [ReadOnly] public bool runningInGame = false; //used by options meny

    private static bool _DEBUG = true;
    private static GameObject _obInstance;
    private static string _POLICE_ENEMY_TAG = "Police";
    private static string _ZOMBIE_ENEMY_TAG = "Zombie";
    private Dictionary<int, Vector3> _scenePlayerPos;
    private List<NPC> _enemyObjects = new List<NPC>(); //List over  Enemy NPCs in-game
    private List<bool> _playerHasVisited = new List<bool>();
    public GameData runningGame = new GameData();
    public float musicVolume = 0.1f;


    public ObjectController() {
        
        // Create list to hold enemy and npc objects during game
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        this._enemyObjects = new List<NPC>();
        this._playerHasVisited = new List<bool>();
        this.runningGame = new GameData();
        ResetPlayerHasVisited(); //Set all visited scenes to false
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetPlayerHasVisited();
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUGGING; TO BE REMOVED
        if ( (Input.GetKeyDown(KeyCode.F11)) || 
             (((Input.GetKeyDown(KeyCode.AltGr)) && (Input.GetKeyDown(KeyCode.S))))
        ) {
            if (_DEBUG) Debug.Log("Catched Save Keypress");
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            playerCaughtByCop();
        }
        
        //DEBUGGING; TO BE REMOVED
        if ( (Input.GetKeyDown(KeyCode.F12)) || 
             ((Input.GetKeyDown(KeyCode.AltGr)) && (Input.GetKeyDown(KeyCode.O)))
        ) {
            if (_DEBUG) Debug.Log("Catched Load Keypress");
            LoadGame();
        }

    }


    void Awake() {
        // Keep this instance alive for the rest of the game
        
        DontDestroyOnLoad(this.gameObject);

        if (_obInstance == null) {
            _obInstance = this.gameObject;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// This method resets the list playerHasVisited. The list indicate
    /// with bool values whether a player has visited a scene. It's can be used
    /// e.g. during loading of a scene; to decide that we should load player
    /// position from a predefined setting or from a list currently running
    /// in-memory (running game). ResetGame() and the scene loading system
    /// has implemented interactions with the list.
    /// </summary>
    private void ResetPlayerHasVisited() {
        this._playerHasVisited = new List<bool>();
        for (int i = 0; i <= SceneLoader.MAX_NUM_SCENES; i++) {
            this._playerHasVisited.Add(false);
        }
    }
    
    /// <summary>
    ///  This function updates a entry inside the playerHasVisited list
    ///  with a specified boolean value. It's used for the reasons like the
    ///  function above.
    /// </summary>
    /// <param name="scene">The scene to set the visited state</param>
    /// <param name="visited">true if visited, false if unvisited</param>
    public void SetPlayerVisitedScene(int scene, bool visited) {
        this._playerHasVisited[scene] = visited;
    }

    /// <summary>
    ///  This function returns a boolean value to indicate if a player
    ///  has visited a specified scene.
    /// </summary>
    /// <param name="scene">The scene index number</param>
    /// <returns>The bool state of the specified scene</returns>
    public bool PlayerHasVisitedScene(int scene) {
        return this._playerHasVisited[scene];
    }
    
    /// <summary>
    ///  This function returns a Vector3 postion object
    ///  for a GameObject in scene with specified tag name
    /// </summary>
    /// <param name="tag"> the tag name</param>
    /// <returns></returns>
    private Vector3 FindPlayerPositionFromTag(string tag) {
        GameObject g = GameObject.FindWithTag(tag);
        Vector3 playerPos;
        if (g is null) {
            // object was not found and doesn't exist
            if (_DEBUG) Debug.Log("Unable to save object, Player not found");
            playerPos = new Vector3(0,0,0);
        } else {
          // found object
          playerPos = g.transform.position;
        }

        return playerPos;
    }

    /// <summary>
    /// This function resets the game data, and also deletes the save json
    /// file if present. It will retain high score value if set in GameData
    /// value
    /// </summary>
    public void ResetGame() {
        
        // needed to load hs from file, because gamedata has 0 score if
        // the player just started the game
        SaveGame hsImporter = new SaveGame();
        GameData hsData = hsImporter.LoadFromFile(SaveType.Highscore);
        if (this.runningGame.keepHighScore) {
            // import new highscore if higher than older saved hs 
            if (this.runningGame.highscore < hsData.highscore) {
                this.runningGame.highscore = hsData.highscore;
            }
            
            // store the highscore value
            SaveGame saveHighScore = new SaveGame(hsData);
            hsData.keepHighScore = true; //must be true to reimport
            saveHighScore.SaveToFile(SaveType.Highscore);
        }
        else {
            hsData.keepHighScore = false;
            SaveGame deleteHs = new SaveGame();
            deleteHs.DeleteSave(SaveType.Highscore);
        }
        
        // Overwrite and reset data
        this._enemyObjects = new List<NPC>();
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        this.runningGame = new GameData();
        ResetPlayerHasVisited();
        SaveGame deleteSave = new SaveGame();
        if (deleteSave.SaveExists(SaveType.Json)) {
            deleteSave.DeleteSave(SaveType.Json);
        }

        // reimport hs state and hs value from hs save
        if (hsData.keepHighScore) {
            this.runningGame.keepHighScore = hsData.keepHighScore;
            this.runningGame.highscore = hsData.highscore;
        }
    }
    /// <summary>
    ///  This methods saves the game
    /// </summary>
    public void SaveGame() {
        if (_DEBUG) Debug.Log("Saving");
        // assign in-game data to save data object; like health, weapons etc
        GameData toBeSaved = this.runningGame;
       Helper load = new Helper();
       int currentScene = load.FindSceneLoaderInScene().GetCurrentScene();
       
       //Stores Relevant Game Data
       toBeSaved.playerHealth = load.FindPlayerHealthInScene().GetCurrentHealth();
       toBeSaved.timeOfDay = load.GetDayControllerInScene().GetDayCycle().ToString();

       //toBeSaved.savedEnemyList = _enemyObjects;
       //toBeSaved.savedPlayerPosition = _scenePlayerPos;
       //toBeSaved.jsonSavedEnemies = convertNpcListToJson(_enemyObjects);
       toBeSaved.WriteToSave(this.FindPlayerPositionFromTag("Player"),currentScene);
       //if (_DEBUG) Debug.Log("Converted NPC-list into JSON:" + toBeSaved.jsonSavedEnemies);

       // save ingame high score
       if (toBeSaved.keepHighScore) {
           GameData hsData = new GameData();
           // checking for largest score is done when creating new game
           hsData.highscore = toBeSaved.highscore;
           SaveGame highScoreSave = new SaveGame(hsData);
           highScoreSave.SaveToFile(SaveType.Highscore);

       }
       else {
          SaveGame highScoreDeleter = new SaveGame(); 
          highScoreDeleter.DeleteSave(SaveType.Highscore);
       }
       
       SaveGame defaultSave = new SaveGame(toBeSaved);
       if (!(defaultSave.SaveToFile(SaveType.Json))) {
           if (_DEBUG) Debug.Log("Saved game went wrong");
       }
       else {
           Helper pauseMenu = new Helper();
           PauseMenu menu = pauseMenu.FindPauseMenuInScene();
           menu.DisplaySuccessfulSave(true);
       }

    }

    /// <summary>
    /// This method loads data from a save into the game (into this instance) 
    /// </summary>
    public void LoadGame() {
        if (_DEBUG) Debug.Log("Loaded saved game");
        SaveGame defaultLoadSlot = new SaveGame(); 
        GameData loaded = defaultLoadSlot.LoadFromFile(SaveType.Json);
        if (_DEBUG) {
            Debug.Log("Loaded GameSave from JSON");
            Debug.Log("Time created: " + loaded.timeCreated);
            Debug.Log("Last accessed: " + loaded.timeAccessed);
        }
        
        //Find sceneloader in scene and set required values before loading
        this._scenePlayerPos[loaded.playerScene] = loaded.GetPlayerPosition();
        this._playerHasVisited[loaded.playerScene] = true;

        //
        Helper load = new Helper();
        DayCycle dayCycle = (DayCycle) Enum.Parse(typeof(DayCycle), loaded.timeOfDay.ToString());
        load.GetDayControllerInScene().SetDayCycle(dayCycle);

        // Assign running data as loaded data
        this.runningGame = loaded;
        Debug.Log("Loading from save into scene " + loaded.playerScene);
        SceneManager.LoadScene(loaded.playerScene);
        //Load PlayerPos (this is not automatically done if target Scene is current Scene
        // because the SceneLoader instance already exist and what that is in start()
        // is not executed.
        //LoadPlayerData(loaded.playerScene); 

    }

    /// <summary>
    ///  Handles player health data storage and presentation
    /// When loading a scene, health value is loaded from memory 
    /// and set to the player object in the scene.
    /// When loadFromList, HealthBar UI is filled with current lvl health 
    /// When exiting a scene, loadFromList must be false,
    ///  to store the players health value in memory. 
    /// </summary>
    /// <param name="loadFromList">true if loading a scene</param>
    private void SetPlayerHealth(bool loadFromList, bool initHealthBarUI) {
        Helper objectHelper = new Helper();
        try {
            Health playerHealth = objectHelper.FindPlayerHealthInScene();
            if (loadFromList) {
                playerHealth.SetHealth(this.runningGame.playerHealth);
                    if (initHealthBarUI) {
                        objectHelper.FindHealthBarInScene().setHealthLevel(
                            playerHealth.GetCurrentHealth());
                    }
            }
            else {
                this.runningGame.playerHealth = playerHealth.GetCurrentHealth();
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    

    /// <summary>
    /// Saves player data (position, health etc) in memory. Player position is saved for the
    /// specified scene
    /// </summary>
    /// <param name="scene">Save slot to write to, normally this is the running scene</param>
    public void WritePlayerData(int scene) {
        this._scenePlayerPos[scene] = FindPlayerPositionFromTag("Player");
        if (_DEBUG) Debug.Log("Saved player coordinates at "
                             + this._scenePlayerPos[scene].ToString() + " for scene " + scene);
        SetPlayerHealth(false, false);
    }
    
    /// <summary>
    /// Loads player data (position, health etc) into a scene from memory. PlayerPosition is loaded
    /// from a list for the specified scene. It also loads current health and updates the player
    /// </summary>
    /// <param name="scene">The selected scene index to load from</param>
    public void LoadPlayerData(int scene) {
        Vector3 result;
        if (_scenePlayerPos is null) { 
            if (_DEBUG) Debug.Log("No player coordinates stored");
        } else {
            if (this._scenePlayerPos.TryGetValue(scene, out result)) {
                if (_DEBUG) Debug.Log("Found coordinates for scene "
                                    + scene + " at " + result.ToString());
                GameObject player = (GameObject)Instantiate(prefabPlayer, result, Quaternion.identity);
                //Health playerHealthScript = player.GetComponent<Health>();
                //playerHealthScript.SetHealth(this.runningGame.playerHealth);
                
            } else {
                if (_DEBUG) Debug.Log("Unable to find player coordinates");
            }
        }
        SetPlayerHealth(true, true);
    }

    /// <summary>
    ///  This function sets a position entry in the scenePlayerPos list
    /// for a specifed scene. Old position is immediately overwritten.
    /// It can be e.g. used when wanting to write a position to load
    /// when switching to a specified scene.
    /// </summary>
    /// <param name="pos">The vector position for the player</param>
    /// <param name="scene">The scene index to write the position into</param>
    public void SetPlayerPos(Vector3 pos, int scene) {
        this._scenePlayerPos[scene] = pos;
    }

    
    /// <summary>
    /// Save all NPCs for the current runniing scene into a specified slot;
    /// coordinates and NPC type is saved 
    /// </summary>
    /// <param name="scene">The slot number to write to
    /// (normally the current scene index)</param>
    public void WriteEnemyPosInScene(int scene) {
        Array policeInScene = GameObject.FindGameObjectsWithTag(_POLICE_ENEMY_TAG);
        Array zombieInScene = GameObject.FindGameObjectsWithTag(_ZOMBIE_ENEMY_TAG);
        //Dictionary<int,Vector3> currentSceneDir = new Dictionary<int, Vector3>();
        WriteEnemiesToList(policeInScene, scene);
        WriteEnemiesToList(zombieInScene, scene);
    }

    /// <summary>
    /// This helper function assists the above function by writing
    /// a specified GameObject array from a scene into the enemyObjects list.
    /// It also verifies that the NPC/Enemy/Police GameObject is valid before
    /// storing it into the list.
    /// </summary>
    /// <param name="enemies">An array of enemies to write into the list</param>
    /// <param name="scene">The scene index to associate the enemies/NPC to</param>
    private void WriteEnemiesToList(Array enemies, int scene) {
        foreach (GameObject enemy in enemies) {
            Debug.Log("Harvesting object " + enemy.name + " from scene " + scene);
            NPC npc = new NPC(enemy, scene);
            // If valid enemy store in dict and remove from scene
            if (npc.valid) {
                _enemyObjects.Add(npc);
                Destroy(enemy);
                Debug.Log("Stored " + npc.getTypeString
                            + " to list for scene " + npc.GetScene()
                            + " " + npc.Position3Axis.ToString());
            }
            else {
                Debug.Log("No valid NPCs to store for scene " + scene);
            }
        }
        if (enemies.Length == 0) {
            Debug.Log("NPC enemies array is also empty in scene" + scene);
        }
        Debug.Log("Total count of stored NPCs: " + _enemyObjects.Count.ToString());
    }
    
    
    /// <summary>
    /// This function loads all saved NPC's for the specified scene
    /// and then spawns them
    /// </summary>
    /// <param name="scene">The scene to load NPC information from</param>
    public void LoadEnemyPosInScene(int scene) {
        if (this._enemyObjects.Count.Equals(0))  {
                    if (_DEBUG) Debug.Log("The ememyObject list is empty");
        } else {
            List<GameObject> spawned = new List<GameObject>();
            for (int i = _enemyObjects.Count - 1; i >= 0; i--) {
                NPC npc = _enemyObjects[i];
                string enemyType = npc.getTypeString;
                if (npc.GetScene().Equals(scene)) {
                    Vector3 newpos = npc.Position3Axis;
                    switch (enemyType) {
                        case "Zombie" :
                            spawned.Add(Instantiate(prefabZombie, newpos, Quaternion.identity));
                            break;
                        
                        case "Police" :
                            spawned.Add(Instantiate(prefabPolice, newpos, Quaternion.identity));
                            break;
                        default:
                            Debug.Log(
                                "Found enemies, but none had a valid sprite");
                            break;
                    }
                    // Remove spawned entities to avoid future duplicates
                    if (_DEBUG) Debug.Log(" Loaded, removing " + npc.getTypeString
                                          + "from the list.");
                    _enemyObjects.Remove(npc);

                    if (spawned.Count.Equals(0)) {
                        Debug.Log("No NPCs were spawned to this scene.");
                    }
                    else {
                        foreach (var gb in spawned) {
                            Debug.Log("Spawned " + gb.name + "at "
                                      + gb.transform.position.ToString());
                        }
                    }
                }
            }
        }
    }

    /* Starts a listening event for respawnPlayerInJail.
     * Then loads the scene where the player will respawn.
     */
    public void playerCaughtByCop() { 
        enableCaptureMessage();
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        SceneManager.sceneLoaded += respawnPlayerInJail;
        SceneManager.LoadScene("Jail");
    }

    /// <summary>
    /// Enables a capture message on the Player UI.
    /// </summary>
    private void enableCaptureMessage() {
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        TMP_Text capturedMessage = null;
        if (playerUI != null) {
            capturedMessage = playerUI.GetComponentInChildren<TMP_Text>();
        }
        if (capturedMessage != null) {
            capturedMessage.enabled = true;
        }
    }

    /* Destroys the player object and creates new position lists.
     * Instantiates a new player object on the coordinates of the spawn.
     */
    private void respawnPlayerInJail(Scene scene, LoadSceneMode mode) {
        this._enemyObjects = new List<NPC>();
        this._playerHasVisited = new List<bool>();
        // quickfix until proper fix in another class - when player dies
        this.runningGame.playerHealth = 8;    
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        Destroy(g);
        ResetPlayerHasVisited();
        this.runningGame.score = 0; //reset score
        SceneManager.sceneLoaded -= respawnPlayerInJail;
    }
    

    /// <summary>
    /// Checks wether player collides with police.
    /// Add 'StartCoroutine("playerCollideWithEnemy");' in Update() to run.
    /// </summary>
    /// <returns>null</returns> if either player, police, or either objects'
    /// colliders are not found.
    private IEnumerator playerCollideWithEnemy() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject police = GameObject.FindGameObjectWithTag("Police");
        if (player == null || police == null) {
            yield return null;
        }
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D policeCollider = player.GetComponent<Collider2D>();
        if (playerCollider == null || policeCollider == null) {
            yield return null;
        }
        if (playerCollider.IsTouching(police.GetComponent<Collider2D>())) {
            playerCaughtByCop();
        }
    }



    /// <summary>
    /// Increments the score with specified amount, returns bool true
    /// if new highscore has been reached
    /// </summary>
    /// <param name="amount">amount to increase/decrease score with</param>
    /// <returns></returns>
    public bool IncrementScore(int amount) {
        int currentScore = this.runningGame.score;
        int highScore = this.runningGame.highscore;
        bool newHigh;

        currentScore += amount;
        if (currentScore < 0) {
            currentScore = 0;
        }

        Debug.Log("Score " + currentScore);

        if (currentScore <= highScore) {
            newHigh = false;
        }
        else { 
            highScore = currentScore;
            Debug.Log("New high " + highScore);
            newHigh = true;
        }
        this.runningGame.score = currentScore;
        this.runningGame.highscore = highScore;
        return newHigh;

    }

    /// <summary>
    /// Finds the object responsible for background music and sets 
    /// the volume to the current value stored in memory
    /// </summary>
    /// <returns>true if successful volume set</returns>
    public bool UpdateBackgroundMusic() {
        bool success = false;
        float volume = this.musicVolume;
        if ((volume <= 1) && (volume >= 0)) {
            GameObject backgroundMusic = GameObject.Find("BackgroundMusic");
            if (!(backgroundMusic is null)) {
                backgroundMusic.GetComponent<AudioSource>().volume = this.musicVolume;
                success = true;
            }
        }

        return success;
    }
}
