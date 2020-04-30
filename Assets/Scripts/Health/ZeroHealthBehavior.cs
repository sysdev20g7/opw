using UnityEngine;

/// <summary>
/// Represents a zero-health behavior for an object.
/// Different types of game objects require different zero-health behaviors.
/// </summary>
public abstract class ZeroHealthBehavior : MonoBehaviour
{

    public abstract void ZeroHealthAction();

}
