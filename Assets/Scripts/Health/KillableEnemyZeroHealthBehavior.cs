using UnityEngine;

/// <summary>
/// Represents the zero-health behavior of 
/// killable enemies.
/// Behavior is to destroy the Unity game object
/// this script is attacted to.
/// </summary>
public class KillableEnemyZeroHealthBehavior : ZeroHealthBehavior
{

    private GameObject enemy;

    void Start()
    {
        enemy = this.gameObject;
    }

    public override void ZeroHealthAction() {
        Destroy(enemy);
    }

}
