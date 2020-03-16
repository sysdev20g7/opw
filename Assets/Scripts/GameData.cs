using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class GameData {
    private List<NPC> _savedEnemyList = new List<NPC>();
    private Dictionary<int, Vector3> _savedPlayerPosition = new Dictionary<int, Vector3>();
    private DateTime _timeCreated;
    private bool _dataSaved;

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
        this._timeCreated = DateTime.Now;
        this._dataSaved = false;
    }

    public void WriteNPCList(List<NPC> npcList) {
        this._savedEnemyList = npcList;
        
    }

    public List<NPC> ReadNPCList() {
        return this._savedEnemyList;
    }

    public void WriteSavedPlayerPositions(Dictionary<int, Vector3> dictionary) {
        this._savedPlayerPosition = dictionary;
    }
    public Dictionary<int, Vector3> LoadSavedPlayerPositions() {
        return this._savedPlayerPosition;
    }

}

