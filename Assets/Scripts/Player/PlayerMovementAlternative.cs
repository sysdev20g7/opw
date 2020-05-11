using UnityEngine;

/// <summary>
/// Grants the player movement in 4 axis and updates Animator
/// </summary>
public class PlayerMovementAlternative : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    public Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Captures Player Input
    void Update() {

        if (!animator.GetBool("IsAttacking")) { 
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.x == 1 ||
                movement.x == -1 ||
                movement.y == 1 ||
                movement.y == -1) {
                animator.SetFloat("LastHorizontal", movement.x);
                animator.SetFloat("LastVertical", movement.y);
            }
        }
        else {
            //Freezes movement of Player Object when attacking.
            rb.velocity = Vector2.zero;
        }
    }

    //Updates the players position
    void FixedUpdate() {
        //Does not allow movement when attacking.
        if (!animator.GetBool("IsAttacking"))
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
