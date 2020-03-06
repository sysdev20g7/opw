using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = System.Object;

/*
 *  The object controller is persistant (runs during the entire game) and
 *  interact with different objects when needed, as e.g. handeling player
 *  position between scene switching.
 *
 *  TO TEST:
 *     1. Add ObjectController prefab to first scene in game
 *     2. Set ObjectController GameObject with GameController tag in meta
 *     3. Set Player in scene with Player-tag in meta (do this for each scene)
 *
 *     4. Set prefix names for sprites to use when spawning npc
 *     5. Set
 *
 *     Writing and recalling player pos works (per 04/03-20 ) but the Camera
 *     won't follow along to the next new position. 
 */
public class ObjectController : MonoBehaviour {

    public GameObject prefabZombie;
    public GameObject prefabPolice;
    private static bool _DEBUG = true;
    private static int _npcType; // 1 == police, 2 == zombie
    private static string _SPRITE_PREFIX_POLICE = "police";
    private static string _SPRITE_PREFIX_ZOMBIE = "goblin";
    private static string _NPC_ENEMY_TAG = "Enemy";
    private Dictionary<int, Vector3> _scenePlayerPos;
    private Dictionary<int, Vector3> _npcForrestPos;
    private Dictionary<int, Vector3> _npcSecretBasePos;
    private Dictionary<int, Dictionary<int, Tuple<int,Vector3>>> _sceneEnemies;
    
    // Start is called before the first frame update
    void Start()
    {
        // Create new dict to store player positions
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        //Create nested dict for scene, <Dict:npcType, pos>
        this._sceneEnemies = new Dictionary<int, Dictionary<int,Tuple<int,Vector3>>>();
        // Load prefabs to be used for spawns in game
        //this.prefabZombie = (GameObject)Resources.Load("Assets/Prefabs/Goblin.prefab");
        //this.prefabPolice = (GameObject)Resources.Load("Assets/Prefabs/Police.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake() {
        // Keep this instance alive for the rest of the game
        DontDestroyOnLoad(this.gameObject);
    }
    
    /*
     *  Save current player position to for a specified scene
     *  @param int scene -- the scene number to store the coordinates in
     */
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

    /*
     *  This function loads the saved position for the player in a specified scene
     * @param int scene -- the scene to load the position for
     */
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

    
    /*
     *  
     */
    public void WriteEnemyPosInScene(int scene) {
        Array enemiesInScene = GameObject.FindGameObjectsWithTag(_NPC_ENEMY_TAG);
        //Dictionary<int,Vector3> currentSceneDir = new Dictionary<int, Vector3>();
        Dictionary<int,Tuple<int,Vector3>> currentSceneDir = new Dictionary<int, Tuple<int, Vector3>>();
        int index = 0;
        foreach (GameObject enemy in enemiesInScene) {
            Debug.Log("Harvesting object " + enemy.name + " from scene " + scene);
            bool validObject = false;
            index++;
            var enemySpriteName = enemy.GetComponent<SpriteRenderer>().sprite.name;
            // Identify what type of npc, and save pos to correct dict
            if (enemySpriteName.StartsWith(_SPRITE_PREFIX_POLICE)) {
                _npcType = 1;
                validObject = true;
            }
            
            else if(enemySpriteName.StartsWith(_SPRITE_PREFIX_ZOMBIE)) {
                _npcType = 2;
                validObject = true;
            }

            // If valid enemy store in dict and remove from scene
            if (validObject) {
                Tuple<int, Vector3> parameter 
                    = new Tuple<int, Vector3>(_npcType,enemy.transform.position);
                //currentSceneDir.Add(parameter);
                currentSceneDir[index] = parameter;
                Destroy(enemy);
                Debug.Log("Stored NPC slot " + index
                            + "; TYPE=" + parameter.Item1 + "Coordinates "
                            + parameter.Item2.ToString());
                
            }
            else {
                Debug.Log("No valid NPCs to store for scene " + scene);
            }

            if (enemiesInScene.Length == 0) {
                Debug.Log("NPC enemies array is also empty in scene" + scene);
            }

        } 
        this._sceneEnemies[scene] = currentSceneDir; //Overwrite old data
    }

    
    public void LoadEnemyPosInScene(int scene) {
        // Open nested working dict for current scene
        Dictionary<int,Tuple<int,Vector3>> currentSceneDir
            = new Dictionary<int, Tuple<int, Vector3>>();
        if (this._sceneEnemies is null ) {
            
        } else {
            
            if (this._sceneEnemies.TryGetValue(scene, out currentSceneDir)) {
                if (currentSceneDir is null) {
                    if (_DEBUG) Debug.Log("No NPC values stored in this dict");
                } else {
                    // For each stored enemy spawn with prefab on saved pos
                    foreach (var enemy in currentSceneDir) {
                        if (_DEBUG) Debug.Log("Loading + " + currentSceneDir.Count 
                            + " entries stored in this dict");
                        Vector3 result = enemy.Value.Item2;
                        int enemyType = enemy.Value.Item1;
                        if (enemyType == 1) {
                            Instantiate(prefabPolice, result, Quaternion.identity);
                        }
                        else if (enemyType == 2) {
                            Instantiate(prefabZombie, result, Quaternion.identity);
                        }
                        Debug.Log("Loaded NPC slot " + enemy.Key.ToString()
                                + "; TYPE=" + enemy.Value.Item1 + "Coordinates "
                                + enemy.Value.Item2.ToString());
                        //Remove entry after spawning
                        currentSceneDir.Remove(enemy.Key);
                    }
                }
            }
            else {
                    if (_DEBUG) Debug.Log("This scene " + scene + " does not have"
                                          + " any NPC coordinates associated with it");
            }
        }
    }
}
