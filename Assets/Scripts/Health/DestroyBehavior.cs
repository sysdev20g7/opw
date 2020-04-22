using UnityEngine;

/// <summary>
/// Represents a destroy behavior for an object.
/// Different types of game objects require different destroy behaviors.
/// </summary>
public abstract class DestroyBehavior : MonoBehaviour
{

    public abstract void destroyObject();

}
