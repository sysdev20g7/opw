using UnityEngine;

/// <summary>
/// Represents a zero-health behavior for an object.
/// Different types of game objects require different zero-health behaviors.
/// </summary>
public abstract class ZeroHealthBehavior : MonoBehaviour
{
    /// <summary>
    /// The action a Game Object must take when it's
    /// health have reached zero.
    /// </summary>
    public abstract void ZeroHealthAction();
}
