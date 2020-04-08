using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCorrectIdleAnimation : MonoBehaviour
{
    private string currentDirection;
    private Rigidbody2D rb;
    private void Update() {
        rb = GetComponent<Rigidbody2D>();
        float velocityX = rb.velocity.x;
        float velocityY = rb.velocity.y;
        bool xNegative = velocityX < 0;
        bool yNegative = velocityY < 0;

        if (xNegative || yNegative) {
            velocityY = System.Math.Abs(velocityY);
            velocityX = System.Math.Abs(velocityX);
            if ((velocityX > velocityY) && xNegative) {
                currentDirection = "Left";
            }
            else if (velocityY > velocityX) {
                currentDirection = "Right";
            }
            else {
                currentDirection = "Down";
            }
        }
        else {
            if (velocityX > velocityY) {
                currentDirection = "Right";
            }
            else {
                currentDirection = "Up";
            }
        }
    }

    public void PlayCorrectAnimation() {
        Animator anim = GetComponent<Animator>();
        switch (currentDirection) {
            case "Up":
                anim.Play("Player_Up_Idle");
                break;

            case "Down":
                anim.Play("Player_Down_Idle");
                break;

            case "Left":
                anim.Play("Player_Left_Idle");
                break;

            case "Right":
                anim.Play("Player_Right_Idle");
                break;
        }
    }
}
