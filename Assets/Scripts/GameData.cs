using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]

///
/// This class holds game data to be used for serilazation ( such as saving/loading)
/// 
public class GameData {
     public static float playerPosZ = 0;
     public string timeCreated;
     public string timeAccessed;
     public string jsonSavedEnemies;
     public int playerScene;
     public float playerPosX, playerPosY; 
     public float cameraPosX, cameraPosY, cameraPosZ;
     public int playerHealth;
     public bool playerWeaponSword;
     
     
     // The datatypes below are not support with seriliazation in json
     // (Unity's JsonUtility)
     public List<NPC> savedEnemyList = new List<NPC>();
     public Dictionary<int, Vector3> savedPlayerPositionList    //TO BE REMOVED
        = new Dictionary<int, Vector3>(); // TO BE REMOVED IF NOT NEEDED


    public GameData() {
        this.timeAccessed = "";
        this.timeCreated = "";
    }
    /// <summary>
    ///  This function writes the player position and scene
    ///  into the save object
    /// </summary>
    /// <param name="playerPos"> Vector3 from player pos</param>
    /// <param name="scene">Scene index</param>
    public void WriteToSave(Vector3 playerPos, int scene) { 
        this.playerPosX = playerPos.x;
        this.playerPosY = playerPos.y;
        Vector3 camPos = Camera.main.transform.position;
        this.cameraPosX = camPos.x;
        this.cameraPosY = camPos.y;
        this.cameraPosZ = camPos.z;
        this.playerScene = scene;
    }

    /// <summary>
    ///  Returns the players position
    /// </summary>
    /// <returns>Vector3 to hold position</returns>
    public Vector3 GetPlayerPosition() {
        return new Vector3(playerPosX,playerPosY,playerPosZ);
    }
    
    
    // TO BE REMOVED -------- BELOW 
    public void WriteToSave(Dictionary<int, Vector3> playerPos) {
        if (playerPos is null) {
            Debug.Log("WARN: Save; player pos dict empty, ignoring..");
        } else {
            this.savedPlayerPositionList = playerPos;
        }
    }
    
    /// <summary>
    /// This writes a List of NPC objects to this instance of SaveGame
    /// (may be removed)
    /// </summary>
    /// <param name="npcList"></param>
    public void WriteToSave(List<NPC> npcList) {
        if ((npcList is null) || (npcList.Count == 0)) {
            Debug.Log("WARN: Save; NPC list empty, ignoring..");
        } else {
            this.savedEnemyList = npcList;
        }
    }
    // TO BE REMOVED -------- END 
}

