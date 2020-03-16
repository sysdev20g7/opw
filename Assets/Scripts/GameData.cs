using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class GameData {
    [SerializeField] private List<NPC> savedEnemyList = new List<NPC>();
    [SerializeField] private Dictionary<int, Vector3> savedPlayerPosition 
        = new Dictionary<int, Vector3>();
    [SerializeField] private DateTime timeCreated;
    [SerializeField] private bool dataSaved;

    public int playerHealth;
    public int[] currentPlayerItems;

    public GameData() {
        this.dataSaved = false;
    }

    public void WriteNPCList(List<NPC> npcList) {
        this.savedEnemyList = npcList;
        dataSaved = true;
        
    }

    public List<NPC> ReadNPCList() {
        return this.savedEnemyList;
    }

    public void WriteSavedPlayerPositions(Dictionary<int, Vector3> dictionary) {
        this.savedPlayerPosition = dictionary;
        dataSaved = true;
    }
    public Dictionary<int, Vector3> LoadSavedPlayerPositions() {
        return this.savedPlayerPosition;
    }

}

