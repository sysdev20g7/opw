using UnityEngine;

/// <summary>
/// Represents the melee attack of the Player Game Object.
/// 
/// The Player Game Object needs an AttackPoint game object as child
/// to determine if an damageable game object is in range. 
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
    /// Controlls how often player can attack within 1 second.
    /// Notifies animator when attacking.
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
    /// Attacks the first enemy in range, if any.
    /// Will also knock back enemy.
    /// </summary>
    private void Attack()
    {
        Collider2D enemy =
                   Physics2D.OverlapCircle(attackLocation.position, attackRange, enemies);
        if (enemy != null) {
            enemy.gameObject.GetComponent<Health>().TakeDamage(attackDmg);
            this.GetComponent<Knockback>().DoKnockback(enemy.gameObject);
            Debug.Log("Attacking " + enemy.tag);
        }
    }

    //Used for Debug. Shows, in editor, the area where player can hit enemy.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
