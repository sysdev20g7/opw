using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : StateMachineBehaviour
{
    private Transform playerPos;
    public float range;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        animator.GetComponent<EnemyPathfinding>().SetIsChasing(animator.GetBool("isFollowing"));
        //START HIGHSCORE
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Vector2.Distance(playerPos.position, animator.transform.position) > range) {
            animator.SetBool("isFollowing", false);
        }

        switch (animator.GetComponent<EnemyVelocityCheck>().FastestDirection()) {
            case "Left":
                animator.SetBool("isRight", false);
                animator.SetBool("isLeft", true);
                animator.SetBool("isUp", false);
                animator.SetBool("isDown", false);
                break;

            case "Right":
                animator.SetBool("isRight", true);
                animator.SetBool("isLeft", false);
                animator.SetBool("isUp", false);
                animator.SetBool("isDown", false);
                break;

            case "Up":
                animator.SetBool("isRight", false);
                animator.SetBool("isLeft", false);
                animator.SetBool("isUp", true);
                animator.SetBool("isDown", false);
                break;

            case "Down":
                animator.SetBool("isRight", false);
                animator.SetBool("isLeft", false);
                animator.SetBool("isUp", false);
                animator.SetBool("isDown", true);
                break;

            default:
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // END HIGHSCORE
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("isRight", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isUp", false);
        animator.SetBool("isDown", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
