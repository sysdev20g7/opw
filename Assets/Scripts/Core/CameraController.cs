using UnityEngine;

/// <summary>
/// Sets the main camera position on the center of the player game object.
/// </summary>
public class CameraController : MonoBehaviour
{

    [SerializeField] private float cameradepth = -20f;
    private GameObject player;


    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// LateUpdate() is called after all Update() functions have been called.
    /// Sets the main camera position to player location after the player has moved.
    /// </summary>
    void LateUpdate() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cameradepth);
    }
}
