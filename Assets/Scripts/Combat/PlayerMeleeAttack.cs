using UnityEngine;

/// <summary>
/// Represents the melee attack of the Player Game Object.
/// 
/// The Player Game Object needs an AttackPoint game object as child
/// to determine if an damageable game object is in range.
/// A player can make Player Game Object attack by pressing the space bar.
/// </summary>
public class PlayerMeleeAttack : MeleeAttack
{
    [SerializeField] private Transform attackLocation;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemies;

    void Start() 
    {
        anim = GetComponent<Animator>();
        if (attackDmg == 0) attackDmg = defaultAttackDmg;
    }

    /// <summary>
    /// Lets player attack when pressing the space bar.
    /// Allows X attacks within 1 second, set by attacksPerSecond.
    /// Notifies animator when attack state changes.
    /// </summary>
    void Update()
    {
        if (Time.time > nextAttackTime) { 
            if (IsAttacking) {
                anim.SetBool("IsAttacking", false);
                IsAttacking = false;
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                anim.SetBool("IsAttacking", true);
                IsAttacking = true;
                Attack();
                nextAttackTime = Time.time + 1f / attacksPerSecond;
            }
        }
    }

    /// <summary>
    /// Attacks all enemies in range, if any.
    /// Will also knock back all enemies.
    /// </summary>
    protected override void Attack() {
        Collider2D[] enemyList =
                   Physics2D.OverlapCircleAll(attackLocation.position, attackRange, enemies);
        foreach(Collider2D enemy in enemyList) {
            Debug.Log("Attacking " + enemy.tag);
            enemy.gameObject.GetComponent<Health>().TakeDamage(attackDmg);
            this.GetComponent<Knockback>().DoKnockback(enemy.gameObject);
        }
    }

    //Used for debugging. Shows in editor the area where player can hit enemy.
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
