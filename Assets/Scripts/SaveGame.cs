using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame {
    public static bool DEBUG = true;
    private GameData _data;
    private static string SAVE_PATH = "game.save";
    private static string jsonFile = Application.persistentDataPath + SAVE_PATH + ".json";
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

    private bool SaveToJsonFile(bool overwrite) {
        bool success = true;
        string json = JsonUtility.ToJson(_data);
        try {
            if (File.Exists(jsonFile)) {
                if (overwrite) { 
                    // Delete existing & overwrite
                    if (DEBUG) Debug.Log("Deleting existing json");
                    File.Delete(jsonFile);
                    File.Create(jsonFile);
                    File.WriteAllText(jsonFile, json);
                    if (DEBUG) Debug.Log("Saved json to: " + jsonFile);
                }
                else {
                    // Do nothing as overwrite is false
                    if (DEBUG) Debug.Log("File exist: " + jsonFile + "not overwriting");
                }
            }
            else {
                // Write new file
                File.Create(jsonFile);
                File.WriteAllText(jsonFile, json);
                if (DEBUG) Debug.Log("Created new json to : " + jsonFile );
            }
        }
        catch (Exception e) {
            success = false;
            Console.WriteLine("Unable to write JSON to file: " + e);
            throw;
        }

        return success;
    }

    private bool LoadFromJsonFile() {
        bool ok = false;
        if (File.Exists(jsonFile)) {
            try {
                string jsonText = File.ReadAllText(jsonFile);
                this._data = JsonUtility.FromJson<GameData>(jsonText);
                ok = true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
        return ok;
    }
    private bool SaveToBinaryFile() {
        bool writeOk = true;
        string saveBinPath = Application.persistentDataPath + SAVE_PATH;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        try {
            FileStream fsWrite = File.Create(saveBinPath);
            binaryFormatter.Serialize(fsWrite, _data);
            fsWrite.Close();
            if (DEBUG) Debug.Log("Saved bin to: " + saveBinPath);
        }
        catch (IOException io) {
           Console.WriteLine("Unable to save - I/O-error: " + io); 
        }
        catch (Exception e) {
            Console.WriteLine("General execption: " + e);
            writeOk = false;
            //throw;
        }

        return writeOk;
    }

    private bool LoadFromBinaryFile() {
        bool readOk = true;
        if (File.Exists(Application.persistentDataPath + SAVE_PATH)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            try {
                FileStream fsRead = File.Open(Application.persistentDataPath
                                          + SAVE_PATH, FileMode.Open);
                this._data = (GameData) binaryFormatter.Deserialize(fsRead);
                fsRead.Close();
            if (DEBUG) Debug.Log("Loaded binary save");
            }
            catch (Exception e) {
                Console.WriteLine(e);
                readOk = false;
                //throw;
            }
        }
        return readOk;
    }

    public bool SaveToFile(int type, bool overwrite) {
        if (DEBUG) Debug.Log("SaveToFile invoked");
        if (type == BINARY) {
            return SaveToBinaryFile();
        } 
        else if (type == JSON) {
            return SaveToJsonFile(overwrite);
        }
        else {
            return false;
        }
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



