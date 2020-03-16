using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame {
    private GameData _data;
    private static string SAVE_PATH = "game.save";

    /// <summary>
    /// Create a new instance for saving/loading the game
    /// </summary>
    public SaveGame() {
        this._data = new GameData(); // Init empty data object
    }

    /// <summary>
    /// Load a GameData object to use as working data set in this instance
    /// </summary>
    /// <param name="save"></param>
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
            Debug.Log("WARN: Save; player pos dict empty, ignoring..");
        } else {
            _data.savedPlayerPosition = playerPos;
        }
    }

    public void WriteToSave(List<NPC> npcList) {
        if ((npcList is null) || (npcList.Count == 0)) {
            Debug.Log("WARN: Save; NPC list empty, ignoring..");
        } else {
            _data.savedEnemyList = npcList;
        }
    }
}



