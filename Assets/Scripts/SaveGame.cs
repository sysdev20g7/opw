﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame {
    private GameData _data;
    private static string SAVE_PATH = "game.save";

    public SaveGame() {
        this._data = new GameData();
    }

    public SaveGame(GameData save) {
        this._data = save;
    }

    public void SaveToFile() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fsWrite = File.Create(Application.persistentDataPath + SAVE_PATH);
        binaryFormatter.Serialize(fsWrite,_data);
        fsWrite.Close();
    }

    public GameData LoadFromFile() {
        if (File.Exists(Application.persistentDataPath + SAVE_PATH)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fsRead = File.Open(Application.persistentDataPath
                                          + SAVE_PATH, FileMode.Open);
            _data = (GameData) binaryFormatter.Deserialize(fsRead);
        }
        else {
            _data = null;
        }
        
        return _data;
    }
    public void WriteToSave(Dictionary<int, Vector3> playerPos) {
        if (playerPos is null) {
            Debug.Log("Unable to write to save; player pos dict empty");
        } else {
            _data.WriteSavedPlayerPositions(playerPos);
        }
    }

    public void WriteToSave(List<NPC> npcList) {
        if ((npcList is null) || (npcList.Count == 0)) {
            Debug.Log("Unable to write to save; npc dict empty");
        } else {
            _data.WriteNPCList(npcList);
        }
    }

    public GameData ReadGameData => _data;
}



