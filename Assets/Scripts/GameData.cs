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
    [SerializeField] public DateTime timeCreated;
    [SerializeField] public DateTime timeAccessed;
    [SerializeField] public List<NPC> savedEnemyList = new List<NPC>();
    [SerializeField] public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
    }

}

