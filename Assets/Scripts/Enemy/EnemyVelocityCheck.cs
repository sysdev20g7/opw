using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityCheck : MonoBehaviour
{
    private Rigidbody2D rb;
    bool yNegative;
    bool xNegative;
    float velocityX;
    float velocityY;
    string direction;

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
