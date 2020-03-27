using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int lastOpenScene; 
    
    private static bool _DEBUG = false;
    private static int JSON = 1, BINARY = 2;
    private static GameObject _obInstance;
    private static string _NPC_ENEMY_TAG = "Enemy";
    private Dictionary<int, Vector3> _scenePlayerPos;
    private List<NPC> _enemyObjects = new List<NPC>(); //List over  Enemy NPCs in-game
    private GameData _runningGame = new GameData();


    public ObjectController() {
        
        // Create list to hold enemy and npc objects during game
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        this._enemyObjects = new List<NPC>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( (Input.GetKeyDown(KeyCode.F11)) || 
             (((Input.GetKeyDown(KeyCode.AltGr)) && (Input.GetKeyDown(KeyCode.S))))
        ) {
            if (_DEBUG) Debug.Log("Catched Save Keypress");
            SaveGame();
        }
        
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
    ///  This function resets the exsisting game data
    ///  and also deletes the save file if present.
    /// </summary>
    public void ResetGame() {
        // Overwrite and reset data
        this._enemyObjects = new List<NPC>();
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        this._runningGame = new GameData();
        SaveGame deleteSave = new SaveGame();
        if (deleteSave.SaveExists(JSON)) {
            deleteSave.DeleteSave(JSON);
        }
    }
    /// <summary>
    ///  This methods saves the game
    /// </summary>
    public void SaveGame() {
        if (_DEBUG) Debug.Log("Saving");
       GameData toBeSaved = new GameData();
       Helper load = new Helper();
       int currentScene = load.FindSceneLoaderInScene().GetCurrentScene();
       //toBeSaved.savedEnemyList = _enemyObjects;
       //toBeSaved.savedPlayerPosition = _scenePlayerPos;
       //toBeSaved.jsonSavedEnemies = convertNpcListToJson(_enemyObjects);
       toBeSaved.WriteToSave(this.FindPlayerPositionFromTag("Player"),currentScene);
       //if (_DEBUG) Debug.Log("Converted NPC-list into JSON:" + toBeSaved.jsonSavedEnemies);
       
       SaveGame defaultSave = new SaveGame(toBeSaved);
       if (!(defaultSave.SaveToFile(1))) {
           if (_DEBUG) Debug.Log("Saved game went wrong");
       }
    }

    /// <summary>
    /// This method loads data from a save into the game (into this instance) 
    /// </summary>
    public void LoadGame() {
        if (_DEBUG) Debug.Log("Loaded saved game");
        SaveGame defaultLoadSlot = new SaveGame(); 
        GameData loaded = defaultLoadSlot.LoadFromFile(1);
        if (_DEBUG) {
            Debug.Log("Loaded GameSave from JSON");
            Debug.Log("Time created: " + loaded.timeCreated);
            Debug.Log("Last accessed: " + loaded.timeAccessed);
        }
        
        //Find sceneloader in scene and set required values before loading
        this._scenePlayerPos[loaded.playerScene] = loaded.GetPlayerPosition();
        Debug.Log("Loading from save into scene " + loaded.playerScene);
        SceneManager.LoadScene(loaded.playerScene);
        //Load PlayerPos (this is not automatically done if target Scene is current Scene
        // because the SceneLoader instance already exist and what that is in start()
        // is not executed.
        //LoadSavedPlayerPos(loaded.playerScene); 
    }
    

    /// <summary>
    /// Save current player position (xzy coord) in running scene to a
    /// specified save slot.
    /// </summary>
    /// <param name="scene">Save slot to write to, normally this is the running scene</param>
    public void WriteSavedPlayerPos(int scene) {
        this._scenePlayerPos[scene] = FindPlayerPositionFromTag("Player");
        if (_DEBUG) Debug.Log("Saved player coordinates at "
                             + this._scenePlayerPos[scene].ToString() + " for scene " + scene);
    }
    
    /// <summary>
    /// This function loads the saved position for the player in a specified scene
    /// </summary>
    /// <param name="scene">The selected scene index to load from</param>
    public void LoadSavedPlayerPos(int scene) {
        Vector3 result;
        if (_scenePlayerPos is null) { 
            if (_DEBUG) Debug.Log("No player coordinates stored");
        } else {
            if (this._scenePlayerPos.TryGetValue(scene, out result)) {
                if (_DEBUG) Debug.Log("Found coordinates for scene "
                                    + scene + " at " + result.ToString());
                GameObject g = GameObject.Find("Player");
                // set player pos to last stored pos and rotation to "no rotation"
                g.transform.SetPositionAndRotation(result,Quaternion.identity );
            } else {
                if (_DEBUG) Debug.Log("Unable to find player coordinates");
            }
        }
    }

    
    /// <summary>
    /// Save all NPCs for the current runniing scene into a specified slot;
    /// coordinates and NPC type is saved 
    /// </summary>
    /// <param name="scene">The slot number to write to
    /// (normally the current scene index)</param>
    public void WriteEnemyPosInScene(int scene) {
        Array enemiesInScene = GameObject.FindGameObjectsWithTag(_NPC_ENEMY_TAG);
        //Dictionary<int,Vector3> currentSceneDir = new Dictionary<int, Vector3>();
        foreach (GameObject enemy in enemiesInScene) {
            Debug.Log("Harvesting object " + enemy.name + " from scene " + scene);
            NPC npc = new NPC(enemy,scene);
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
        if (enemiesInScene.Length == 0) {
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
    
    //--------------TO BE REMOVED IF NOT NEEDED --------------//
    // Start
    
    /// <summary>
    ///  This function converts a List<NPC> with multiple NPCs
    ///  into a json-string
    /// </summary>
    /// <param name="source"></param>
    /// <returns>json string</returns>
    private string convertNpcListToJson(List<NPC> source) {
        string json = "";

        for (int i = source.Count - 1; i >= 0; i--) {
            NPC npc = source[i];
            string enemyType = npc.getTypeString;
            json += JsonUtility.ToJson(source);
            Debug.Log("Appended json:" + json);
        }

        return json;
    }
    //END-----------TO BE REMOVED IF NOT NEEDED --------------//
    
    /// <summary>
    /// This method will convert a json string to a NPC list (might not be needed)
    /// </summary>
    /// <param name="json"></param>
    /// <returns>returns a game object filled with data</returns>
    private GameData convertJsonToNpcList(string json) {
        GameData gdFromJson = JsonUtility.FromJson<GameData>(json);
        return gdFromJson;
    }
}
