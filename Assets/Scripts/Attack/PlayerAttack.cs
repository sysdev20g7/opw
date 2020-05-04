using UnityEngine;

public class PlayerAttack : Attack
{
    [SerializeField]
    private Transform attackLocation;
    [SerializeField]
    private float attackRange;
    [SerializeField] LayerMask enemies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime <= startTimeAttack) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("ATTACKED");
                anim.SetBool("IsAttacking", true);
                Collider2D enemy =
                    Physics2D.OverlapCircle(attackLocation.position, attackRange, enemies);
                if (enemy != null) {
                    enemy.gameObject.GetComponent<Health>().TakeDamage(attackDmg);
                }
            }
        attackTime = startTimeAttack;
        }
        else {
            attackTime -= Time.deltaTime;
            anim.SetBool("IsAttacking", false);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}
