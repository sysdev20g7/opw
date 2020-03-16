using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class GameData {
    public List<NPC> savedEnemyList = new List<NPC>();

    public Dictionary<int, Vector3> savedPlayerPosition
        = new Dictionary<int, Vector3>();
    [SerializeField]
    private DateTime _timeCreated;

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

