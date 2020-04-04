using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the action on the player when a specific GameObject's collider
/// enters the players Collision space.
/// </summary>
public class PlayerCollision : MonoBehaviour {

    [SerializeField]
    private ObjectController oc;

    /// <summary>
    /// Defines that when a police object's collision space enters the players collision
    /// space, the player is caught by the cop and respawned.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
      //  if (oc != null) { 
      //      if (collision.gameObject.tag == "Police") {
      //          oc.playerCaughtByCop();
      //      }
      //  }
    }
}
