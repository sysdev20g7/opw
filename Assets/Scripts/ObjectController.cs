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
    
    // Start is called before the first frame update
    void Start()
    {
        this._scenePlayerPos = new Dictionary<int, Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    
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

    public void LoadSavedPlayerPos(int scene) {
        Vector3 result;
        if (this._scenePlayerPos.Count == 0) { 
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
