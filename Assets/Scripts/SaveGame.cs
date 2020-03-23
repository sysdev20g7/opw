using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
///  This class contains methods to perform save/load to files,
///  either binary or json.
/// </summary>
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

    /// <summary>
    ///  Write a save to a json file
    /// </summary>
    /// <returns>boolean, true if success</returns>
    private bool SaveToJsonFile() {
        if (DEBUG) Debug.Log("==== Writing JSON to file : " + jsonFile);
        bool success = true;
        this._data.timeCreated = DateTime.Now.ToString();
        string json = JsonUtility.ToJson(this._data);
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

    /// <summary>
    ///  Load a save from a json file
    /// </summary>
    /// <returns>boolean, true if success</returns>
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
                this._data.timeAccessed = DateTime.Now.ToString();
                jsonText = JsonUtility.ToJson(this._data);
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
    /// <summary>
    /// Save a binary to file
    /// </summary>
    /// <returns>bool returns true if succsessful save</returns>
    private bool SaveToBinaryFile() {
        bool writeOk = true;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        try {
            FileStream fsWrite = File.Create(binaryFile);
            this._data.timeCreated = DateTime.Now.ToString();
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
            //throw;
        }

        return writeOk;
    }

    /// <summary>
    /// Load a binary save from file
    /// </summary>
    /// <returns>true/false bool if successful</returns>
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
                this._data.timeAccessed = DateTime.Now.ToString();
                if (DEBUG) Debug.Log("Updated save with access time");
                binaryFormatter.Serialize(fsRead, _data);
                fsRead.Close();
            }
            catch (Exception e) {
                Debug.Log(e);
                readOk = false;
                //throw;
            }
        }
        return readOk;
    }

    /// <summary>
    /// This function saves a game to file, with specified type
    /// </summary>
    /// <param name="type">JSON = 1, BIN = 2</param>
    /// <returns></returns>
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

    /// <summary>
    /// This function loads a saved game from file
    /// </summary>
    /// <param name="type">JSON = 1, BIN = 2</param>
    /// <returns></returns>
    public GameData LoadFromFile(int type) {
        GameData data = new GameData();
        if (type == BINARY) {
            if (LoadFromBinaryFile()) {
                data = this._data;
            }
        } 
        else if (type == JSON) {
            if (LoadFromJsonFile()) {
                data = this._data;
            }
        }

        return data;
    }
    
    
}



