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
    public static bool DEBUG = false;
    private GameData _data;
    private static string SAVE_PATH = "/gamedata";
    private string jsonFile = Application.persistentDataPath + SAVE_PATH + ".json";
    private string jsonHScoreFile =  Application.persistentDataPath + "/highscore"+ ".json";


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
    /// Delete the saved game file
    /// </summary>
    /// <param name="type"></param>
    public void DeleteSave(SaveType savetype) {
        if (savetype == SaveType.Json) {
            File.Delete(jsonFile);
        } 
        else if (savetype == SaveType.Highscore) {
            File.Delete(jsonHScoreFile);
        } 
    }

    /// <summary>
    /// Check if a game save exists
    /// </summary>
    /// <param name="type">type of game, 1 = JSON</param>
    /// <returns>true if save exists</returns>
    public bool SaveExists(SaveType type) {
        bool exist = false;
        if (type == SaveType.Json) {
            exist = File.Exists(jsonFile);
        }
        else if (type == SaveType.Highscore) {
            exist = File.Exists(jsonHScoreFile);
        }

        return exist;
    }

    /// <summary>
    ///  Write a save to a json file
    /// </summary>
    /// <returns>boolean, true if success</returns>
    private bool SaveToJsonFile(string path) {
        if (DEBUG) Debug.Log("==== Writing JSON to file : " + path);
        bool success = true;
        this._data.timeCreated = DateTime.Now.ToString();
        string json = JsonUtility.ToJson(this._data);
        if (DEBUG) Debug.Log("Prepared JSON before save: " + json);
        try {
            // Write new file & owerwrite if already exsisiting
            File.WriteAllText(path, json);
            if (DEBUG) Debug.Log("Wrote JSON data to : " + path );
            Debug.Log("Saved game to " + path.ToString());
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
    private bool LoadFromJsonFile(string path) {
        if (DEBUG) Debug.Log("==== LOAD JSON from file : " + path);
        bool ok = false;
        if (File.Exists(path)) {
            if (DEBUG) Debug.Log("Found file at: " + path);
            try {
                string jsonText = File.ReadAllText(path);
                if (DEBUG) Debug.Log("Parsing JSON to GameObject: " + jsonText);
                this._data = JsonUtility.FromJson<GameData>(jsonText);
                // Log last time accessed to game save file
                this._data.timeAccessed = DateTime.Now.ToString();
                jsonText = JsonUtility.ToJson(this._data);
                File.WriteAllText(path, jsonText);
                if (DEBUG) Debug.Log("Updated accessed timestamp: " + this._data.timeAccessed);
                ok = true;
            }
            catch (Exception e) {
                if (DEBUG) Debug.Log("ERROR: General exception: " + e);
                //throw;
            }
        }
        else {
            if (DEBUG) Debug.Log("Unable to find file at " + path);
        }

        if (ok) {
            Debug.Log("Loaded game from " + path.ToString());
        }
        return ok;
    }


    /// <summary>
    /// This function saves a game to file, with specified type
    /// </summary>
    /// <param name="type">JSON = 1, BIN = 2</param>
    /// <returns></returns>
    public bool SaveToFile(SaveType type) {
        bool savedOk = false;
        if (DEBUG) Debug.Log("SaveToFile invoked");
        if (type == SaveType.Json) {
            savedOk = SaveToJsonFile(jsonFile);
        } 
        else if (type == SaveType.Highscore) {
            savedOk = SaveToJsonFile(jsonHScoreFile);
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
    public GameData LoadFromFile(SaveType type) {
        GameData data = new GameData();
        if (type == SaveType.Highscore) {
            if (LoadFromJsonFile(jsonHScoreFile)) {
                data = this._data;
            }
        } 
        else if (type == SaveType.Json) {
            if (LoadFromJsonFile(jsonFile)) {
                data = this._data;
            }
        }

        return data;
    }
    
    
}



