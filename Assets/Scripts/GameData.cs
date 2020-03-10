using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public static class GameData {
    private static int _playerHealth;
    private static int[] _currentPlayerItems;

    /*
     *  This function stores the player health to a static variable
     * @param health - the health value
     */
    public static int PlayerHealth {
        get {
            return _playerHealth;
        }
        set {
            _playerHealth = value;
        }
    }
   
    
    /*
     *  This function writes the current player items to a static array
     *  @ param itemArray -- an array of items to store
     */
    public static void SetCurrentPlayerItems(int[] itemArray) {
        _currentPlayerItems = itemArray;
    }
}


