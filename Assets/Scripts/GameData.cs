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
    [SerializeField] private DateTime _timeCreated;
    [SerializeField] public List<NPC> savedEnemyList = new List<NPC>();

    public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
        this._timeCreated = DateTime.Now;
    }

    /// <summary>
    /// Get DateTime of when this save was created
    /// </summary>
    public DateTime TimeCreated => _timeCreated;
}

