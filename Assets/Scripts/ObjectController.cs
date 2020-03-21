using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
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
    private static bool _DEBUG = true;
    private static string _NPC_ENEMY_TAG = "Enemy";
    [SerializeField] private Dictionary<int, Vector3> _scenePlayerPos;
    [SerializeField] private List<NPC> _enemyObjects = new List<NPC>(); //List over  Enemy NPCs in-game
    private GameData _runningGame = new GameData();
    
    // Start is called before the first frame update
    void Start()
    {
        // Create list to hold enemy and npc objects during game
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        this._enemyObjects = new List<NPC>();
        // Load prefabs to be used for spawns in game
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
    }
    
    /*
     *  Save current player position to for a specified scene
     *  @param int scene -- the scene number to store the coordinates in
     */
    /// <summary>
    /// Save current player position (xzy coord) in running scene to a
    /// specified save slot.
    /// </summary>
    /// <param name="scene">Save slot to write to, normally this is the running scene</param>
    public void WriteSavedPlayerPos(int scene) {
        GameObject g = GameObject.FindWithTag("Player");
        if (g is null ) {
            // object was not found and doesn't exist
            if (_DEBUG) Debug.Log("Unable to save object, Player not found");
        } else {
            // object was found
            Vector3 pos = g.transform.position;
            this._scenePlayerPos[scene] = pos;
            if (_DEBUG) Debug.Log("Saved player coordinates at "
                                 + pos.ToString() + " for scene " + scene);
        }
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

    private void SaveGame() {
        if (_DEBUG) Debug.Log("Saving");
       GameData toBeSaved = new GameData();
       toBeSaved.savedEnemyList = _enemyObjects;
       toBeSaved.savedPlayerPosition = _scenePlayerPos;
       
       SaveGame defaultSave = new SaveGame(toBeSaved);
       if (!(defaultSave.SaveToFile(1))) {
           if (_DEBUG) Debug.Log("Saved game went wrong");
       }
    }

    private void LoadGame() {
        if (_DEBUG) Debug.Log("Loaded saved game");
        SaveGame defaultLoadSlot = new SaveGame(); 
        GameData loaded = defaultLoadSlot.LoadFromFile(1);
        if (_DEBUG) {
            Debug.Log("Loaded GameSave from JSON");
            Debug.Log("Time created: " + loaded.timeCreated);
            Debug.Log("Last accessed: " + loaded.timeAccessed);
        }
        
        // populate player and npcs from save
        this._enemyObjects = loaded.savedEnemyList;
        this._scenePlayerPos = loaded.savedPlayerPosition;
    }
    
}
