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
     public string timeCreated;
     public string timeAccessed;
     public string jsonSavedEnemies;
     public int playerScene;
     public float playerPosX, playerPosY; 
     public float cameraPosX, cameraPosY, cameraPosZ;
     public int playerHealth;
     public int[] currentPlayerItems;
     
     // The datatypes below are not support with seriliazation in json
     // (Unity's JsonUtility)
     public List<NPC> savedEnemyList = new List<NPC>();
     public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();


    public GameData() {
        this.timeAccessed = "";
        this.timeCreated = "";
    }
    public void WriteToSave(Vector3 playerPos) { 
        this.playerPosX = playerPos.x;
        this.playerPosY = playerPos.y;
        Vector3 camPos = Camera.main.transform.position;
        this.cameraPosX = camPos.x;
        this.cameraPosY = camPos.y;
        this.cameraPosZ = camPos.z;
    }
    
    public void WriteToSave(Dictionary<int, Vector3> playerPos) {
        if (playerPos is null) {
            Debug.Log("WARN: Save; player pos dict empty, ignoring..");
        } else {
            this.savedPlayerPosition = playerPos;
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
}

