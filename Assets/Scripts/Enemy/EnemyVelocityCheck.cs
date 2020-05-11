using UnityEngine;

/// <summary>
/// Script for cheking in which direction the enemy
/// is moving tha fastest.
/// </summary>
public class EnemyVelocityCheck : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool yNegative;
    private bool xNegative;
    private float velocityX;
    private float velocityY;
    private string direction;

    /// <summary>
    /// Finds out which direction has the highest velocity.
    /// </summary>
    /// <returns>The direction with the highest velocity.</returns>
    public string FastestDirection() {
        rb = GetComponent<Rigidbody2D>();
        velocityX = rb.velocity.x;
        velocityY = rb.velocity.y;
        xNegative = velocityX < 0;
        yNegative = velocityY < 0;

        if (xNegative || yNegative) {
            velocityY = System.Math.Abs(velocityY);
            velocityX = System.Math.Abs(velocityX);
            if ((velocityX > velocityY) && xNegative) {
                direction = "Left";
            }
            else if (velocityY > velocityX) {
                direction = "Right";
            }
            else {
                direction = "Down";
            }
        }
        else {
            if (velocityX > velocityY) {
                direction = "Right";
            }
            else {
                direction = "Up";
            }
        }
        return direction;
    }
}
