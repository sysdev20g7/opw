using UnityEngine;

/// <summary>
/// Represents the destroy beavior of the player.
/// Added to Player object along with Health Script.
/// </summary>
public class PlayerDestroyBehavior : DestroyBehavior {
    private ObjectController objectController;

    void Start() {
        objectController = GameObject.FindObjectOfType<ObjectController>();
    }

    public override void destroyObject() {
        //not that a generic method called ^^
        if (objectController != null) {
            objectController.playerCaughtByCop();
        }
    }
}

