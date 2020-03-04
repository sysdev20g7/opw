using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameData {
    private static int _playerHealth;
    private static int[] _currentPlayerItems;

    /*
     *  This function stores the player health to a static variable
     * @param health - the health value
     */
    public static void SetPlayerHealth(int health) {
        _playerHealth = health;
    }
    
    /*
     *  This function stores the player health to a static variable
     * @param health - the health value
     */
    public static int GetPlayerHealth() {
        return _playerHealth;
    }
    
    /*
     *  This function reads an static array with stored items and
     *  returns them for use on the player
     */
    public static int[] GetCurrentPlayerItems() {
        return _currentPlayerItems;
    }
    
    /*
     *  This function writes the current player items to a static array
     *  @ param itemArray -- an array of items to store
     */
    public static void SetCurrentPlayerItems(int[] itemArray) {
        _currentPlayerItems = itemArray;
    }
}


