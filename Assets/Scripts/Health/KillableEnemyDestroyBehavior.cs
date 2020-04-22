using UnityEngine;

/// <summary>
/// Represents the destroy behavior of 
/// killable enemies.
/// </summary>
public class KillableEnemyDestroyBehavior : DestroyBehavior
{

    private GameObject enemy;

    void Start()
    {
        enemy = this.gameObject;
    }

    public override void destroyObject() {
        Destroy(enemy);
    }

}
