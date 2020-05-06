/// <summary>
/// Represents the zero-health behavior of killable enemies.
/// Destroy the Unity Game Object this script is attacted to.
/// </summary>
public class KillableEnemyZeroHealthBehavior : ZeroHealthBehavior
{
    public override void ZeroHealthAction() {
        Destroy(gameObject);
    }
}
