﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// This class holds game data to be used for serilazation ( such as saving/loading)
/// </summary>
/// <summary>
/// This class holds game data to be used for serilazation ( such as saving/loading)
/// </summary>
[System.Serializable]
public class GameData {
     public static float playerPosZ = 0;
     public string timeCreated;
     public string timeAccessed;
     public string jsonSavedEnemies;
     public int playerScene;
     public float playerPosX, playerPosY; 
     public float cameraPosX, cameraPosY, cameraPosZ;
     public string timeOfDay;
     public int playerHealth;
     public bool playerWeaponSword;
     public int highscore = 0;
     public int score = 0;
     public bool keepHighScore = true;
     
     
     // The datatypes below are not support with seriliazation in json
     // (Unity's JsonUtility)
     public List<NPC> savedEnemyList = new List<NPC>();
     public Dictionary<int, Vector3> savedPlayerPositionList    //TO BE REMOVED
        = new Dictionary<int, Vector3>(); // TO BE REMOVED IF NOT NEEDED


    public GameData() {
        this.timeAccessed = "";
        this.timeCreated = "";
    }
    /// <summary>
    ///  This function writes the player position and scene
    ///  into the save object
    /// </summary>
    /// <param name="playerPos"> Vector3 from player pos</param>
    /// <param name="scene">Scene index</param>
    public void WriteToSave(Vector3 playerPos, int scene) { 
        this.playerPosX = playerPos.x;
        this.playerPosY = playerPos.y;
        Vector3 camPos = Camera.main.transform.position;
        this.cameraPosX = camPos.x;
        this.cameraPosY = camPos.y;
        this.cameraPosZ = camPos.z;
        this.playerScene = scene;
    }

    /// <summary>
    ///  Returns the players position
    /// </summary>
    /// <returns>Vector3 to hold position</returns>
    public Vector3 GetPlayerPosition() {
        return new Vector3(playerPosX,playerPosY,playerPosZ);
    }
}

