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
     public List<NPC> savedEnemyList = new List<NPC>();
     public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
        this.timeAccessed = "";
        this.timeCreated = "";
    }
}

