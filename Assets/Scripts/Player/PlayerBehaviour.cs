using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetAxisRaw("Vertical") > 0) {
            //animator.SetTrigger("Up");
            animator.Play("Player_Up_Walk");
        } 
        if (Input.GetAxisRaw("Vertical") < 0) {
            //animator.SetTrigger("Down");
            animator.Play("Player_Down_Walk");
        }
        if (Input.GetAxisRaw("Horizontal") > 0) {
            //animator.SetTrigger("Right");
            animator.Play("Player_Right_Walk");
        } 
        if (Input.GetAxisRaw("Horizontal") < 0) {
            //animator.SetTrigger("Left");
            animator.Play("Player_Left_Walk");

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
