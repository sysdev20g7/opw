using UnityEngine;

/// <summary>
/// Represents the zero-health beavior of the player.
/// Added to Player object along with Health Script.
/// Calls on Object Controller to restart game.
/// </summary>
public class PlayerZeroHealthBehavior : ZeroHealthBehavior {
    private ObjectController objectController;

    void Start() {
        objectController = GameObject.FindObjectOfType<ObjectController>();
    }

    public override void ZeroHealthAction() {
        //not that a generic method called ^^
        if (objectController != null) {
            objectController.playerCaughtByCop();
        }
    }
}

