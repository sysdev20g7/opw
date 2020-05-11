using UnityEngine;

/// <summary>
/// Represents the zero-health behavior of killable enemies.
/// Destroy the Unity Game Object this script is attacted to.
/// </summary>
public class KillableEnemyZeroHealthBehavior : ZeroHealthBehavior
{
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Destroys the Unity Game Object the script is a component of.
    /// </summary>
    public override void ZeroHealthAction() {

        Helper helper = new Helper();
        helper.FindHighScoreInScene().runScore = false;
        anim.SetBool("IsFollowing", false);
        Destroy(gameObject);
    }
}
