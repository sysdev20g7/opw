using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectController : MonoBehaviour {

    private static bool DEBUG = true;
    private Dictionary<int, Vector3> _scenePlayerPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _scenePlayerPos = new Dictionary<int, Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }


    public void WriteSavedPlayerPos(int scene) {
        GameObject g = GameObject.Find("Player");
        if (g is null ) {
            // object was not found and doesn't exist
            if (DEBUG) Debug.Log("Unable to save object, Player not found");
        } else {
            // object exists, we read position and add to dir
            if (DEBUG) Debug.Log("Saved player coordinates");
           _scenePlayerPos.Add(scene,g.transform.position);
           
        }
    }

    public Vector3 LoadSavedPlayerPos(int scene) {
        Vector3 result;
        try {
            _scenePlayerPos.TryGetValue(scene,out result);
        }
        catch (Exception e) {
            if (DEBUG) Debug.Log("Unable to find player coordinates");
            //Console.WriteLine(e);
            throw;
        }
        if (DEBUG) Debug.Log("Player coordinates loaded:" + result.ToString());
        return result;
    }
}
