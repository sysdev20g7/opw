using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
 *     Writing and recalling player pos works (per 04/03-20 ) but the Camera
 *     won't follow along to the next new position. 
 */
public class ObjectController : MonoBehaviour {

    private static bool DEBUG = true;
    private Dictionary<int, Vector3> _scenePlayerPos;
    private Dictionary<int, Vector3> _npcForrestPos;
    private Dictionary<int, Vector3> _npcSecretBasePos;
    
    // Start is called before the first frame update
    void Start()
    {
        // Create new dict to store player positions
        this._scenePlayerPos = new Dictionary<int, Vector3>();
        //Create dict for NPC last character positions 
        this._npcForrestPos = new Dictionary<int, Vector3>();
        this._npcSecretBasePos = new Dictionary<int, Vector3>();
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
            if (DEBUG) Debug.Log("Unable to save object, Player not found");
        } else {
            // object was found
            Vector3 pos = g.transform.position;
            this._scenePlayerPos[scene] = pos;
            if (DEBUG) Debug.Log("Saved player coordinates at "
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
            if (DEBUG) Debug.Log("No player coordinates stored");
        } else {
            if (this._scenePlayerPos.TryGetValue(scene, out result)) {
                if (DEBUG) Debug.Log("Found coordinates for scene "
                                    + scene + " at " + result.ToString());
                GameObject g = GameObject.Find("Player");
                // set player pos to last stored pos and rotation to "no rotation"
                g.transform.SetPositionAndRotation(result,Quaternion.identity );
            } else {
                if (DEBUG) Debug.Log("Unable to find player coordinates");
            }
        }
        
    }
}
