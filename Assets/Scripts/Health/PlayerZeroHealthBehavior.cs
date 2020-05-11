using UnityEngine;

/// <summary>
/// Represents the zero-health beavior of the Player Game Object
/// Added to Player object along with Health Script.
/// Calls on Object Controller to restart game.
/// </summary>
public class PlayerZeroHealthBehavior : ZeroHealthBehavior {
    private ObjectController objectController;

    void Start() 
    {
        objectController = GameObject.FindObjectOfType<ObjectController>();
    }

    /// <summary>
    /// Calls on Object Controller to restart game.
    /// </summary>
    public override void ZeroHealthAction() {
        if (objectController != null) {
            objectController.playerCaughtByCop();
        }
    }
}

