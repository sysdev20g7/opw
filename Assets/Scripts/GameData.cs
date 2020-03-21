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
     public DateTime? timeCreated;
     public DateTime? timeAccessed = null;
     public List<NPC> savedEnemyList = new List<NPC>();
     public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
        this.timeAccessed = null;
        this.timeCreated = null;
    }


    public String getTimeAccessed() {
        if (this.timeAccessed is null) {
            return "Never";
        }
        else {
            return timeAccessed.ToString();
        }
    }
}

