using UnityEngine;

/// <summary>
/// Defines the action on the player when a few specific GameObject's collider
/// enters the players Collision space.
/// </summary>
public class PlayerCollision : MonoBehaviour {

    [SerializeField]
    private ObjectController oc;


    private void Start() {
        if (oc == null) {
            oc = GameObject.FindObjectOfType<ObjectController>();
        }
    }   
    /// <summary>
    /// Defines that when an enemy object's collision space enters the players collision
    /// space, the player is caught by the enemy and respawned, handled by ObjectController.
    /// 
    /// Note. This implementation is both coupled to the ObjectController
    /// and not generalized as we've not abstracted Police, Zombie and EvilTree to Enemy.
    /// Not generalised at this point as other code is reliant on the seperate tags. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        if (oc != null) {
            if (collision.gameObject.tag == "Police" ||
                collision.gameObject.tag == "Zombie" ||
                collision.gameObject.tag == "EvilTree") {
                oc.playerCaughtByCop();
            }
        }
    }
}
