using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCorrectIdleAnimation : MonoBehaviour
{
    private string currentDirection = "Down";

    public void SetCurrentDirection(string direction) {
        currentDirection = direction;
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
