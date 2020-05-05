using UnityEngine;

public class PlayerMeleeAttack : MeleeAttack
{
    [SerializeField]
    private Transform attackLocation;
    [SerializeField]
    private float attackRange;
    [SerializeField] 
    private LayerMask enemies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (attackDmg == 0) attackDmg = defaultAttackDmg;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttackTime) { 
            if (IsAttacking) {

                attackCounter -= Time.deltaTime;
                if (attackCounter <= 0) {
                    anim.SetBool("IsAttacking", false);
                    IsAttacking = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("ATTACKED");
                attackTime = attackCounter;
                IsAttacking = true;
                anim.SetBool("IsAttacking", true);
                Collider2D enemy =
                    Physics2D.OverlapCircle(attackLocation.position, attackRange, enemies);
                if (enemy != null) {
                    enemy.gameObject.GetComponent<Health>().TakeDamage(attackDmg);
                    this.GetComponent<Knockback>().DoKnockback(enemy.gameObject);
                    Debug.Log("Attacking " + enemy.tag);
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
