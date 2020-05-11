using UnityEngine;

/// <summary>
/// Represents the melee attack of an Enemy Game Object.
/// An Enemy Game Object can only hurt the Player Game Object.
/// The Enemy only hurts the player if the player collides with the enemy.
/// </summary>
public class EnemyMeleeAttack : MeleeAttack
{
    private Collision2D playerCollision;

    void Start()
    {
        if (attackDmg == 0) attackDmg = defaultAttackDmg;
    }

    /// <summary>
    /// Attacks the colliding player object.
    /// </summary>
    protected override void Attack() {
        Health playerHealth =
            playerCollision.gameObject.GetComponent<Health>();
        if (playerHealth != null) {
            playerHealth.TakeDamage(attackDmg);
        }
    }

    /// <summary>
    /// Checks wether the colliding object is the player.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            playerCollision = collision;
            Attack();
        }
    }
}
