using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame {
    public static bool DEBUG = true;
    private GameData _data;
    private static string SAVE_PATH = "/gamedata";
    private string jsonFile = Application.dataPath + SAVE_PATH + ".json";
    private string binaryFile =  Application.dataPath + SAVE_PATH + ".save";
    private static int JSON = 1;
    private static int BINARY = 2;

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

    private bool SaveToJsonFile() {
        if (DEBUG) Debug.Log("==== Writing JSON to file : " + jsonFile);
        bool success = true;
        this._data.timeCreated = DateTime.Now;
        string json = JsonUtility.ToJson(_data);
        if (DEBUG) Debug.Log("Prepared JSON before save: " + json);
        try {
            // Write new file & owerwrite if already exsisiting
            File.WriteAllText(jsonFile, json);
            if (DEBUG) Debug.Log("Wrote JSON data to : " + jsonFile );
        }
        catch (Exception e) {
            success = false;
            Debug.Log("Error: Fault occured: " + e);
            //throw;
        }

        return success;
    }

    private bool LoadFromJsonFile() {
        if (DEBUG) Debug.Log("==== LOAD JSON from file : " + jsonFile);
        bool ok = false;
        if (File.Exists(jsonFile)) {
            if (DEBUG) Debug.Log("Found file at: " + jsonFile);
            try {
                string jsonText = File.ReadAllText(jsonFile);
                if (DEBUG) Debug.Log("Parsing JSON to GameObject: " + jsonText);
                this._data = JsonUtility.FromJson<GameData>(jsonText);
                // Log last time accessed to game save file
                this._data.timeAccessed = DateTime.Now;
                File.WriteAllText(jsonFile, jsonText);
                if (DEBUG) Debug.Log("Updated accessed timestamp: " + this._data.timeAccessed);
                ok = true;
            }
            catch (Exception e) {
                if (DEBUG) Debug.Log("ERROR: General exception: " + e);
                //throw;
            }
        }
        else {
            if (DEBUG) Debug.Log("Unable to find file at " + jsonFile);
        }
        return ok;
    }
    private bool SaveToBinaryFile() {
        bool writeOk = true;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        try {
            FileStream fsWrite = File.Create(binaryFile);
            binaryFormatter.Serialize(fsWrite, _data);
            fsWrite.Close();
            if (DEBUG) Debug.Log("Saved bin to: " + binaryFile);
        }
        catch (IOException io) {
           Debug.Log("Unable to save binary - I/O-error: " + io); 
        }
        catch (Exception e) {
            Debug.Log("Unable to save binary, because: " + e);
            writeOk = false;
            throw;
        }

        return writeOk;
    }

    private bool LoadFromBinaryFile() {
        bool readOk = true;
        if (File.Exists(binaryFile)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try {
                FileStream fsRead = File.Open(binaryFile, FileMode.Open);
                this._data = (GameData) binaryFormatter.Deserialize(fsRead);
                fsRead.Close();
            if (DEBUG) Debug.Log("Loaded binary save");
            if (DEBUG) Debug.Log("Loaded bin from: " + binaryFile);
            }
            catch (Exception e) {
                Debug.Log(e);
                readOk = false;
                //throw;
            }
        }
        return readOk;
    }

    public bool SaveToFile(int type) {
        bool savedOk = false;
        if (DEBUG) Debug.Log("SaveToFile invoked");
        if (type == BINARY) {
            savedOk = SaveToBinaryFile();
        } 
        else if (type == JSON) {
            savedOk = SaveToJsonFile();
        }else {
            savedOk = false;
        }

        return savedOk;
    }

    public GameData LoadFromFile(int type) {
        GameData data = new GameData();
        if (type == BINARY) {
            if (LoadFromBinaryFile()) {
                this._data = data;
            }
        } 
        else if (type == JSON) {
            if (LoadFromJsonFile()) {
                this._data = data;
            }
        }

        return data;
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



