using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour {

    private MoveSpots patrol;
    public float speed;
    private int randomSpot;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        patrol = GameObject.FindGameObjectWithTag("PatrolSpots").GetComponent<MoveSpots>();
        randomSpot = Random.Range(0, patrol.movespots.Length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(Vector2.Distance(animator.transform.position, patrol.movespots[randomSpot].position) > 0.2f) {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrol.movespots[randomSpot].position, speed * Time.deltaTime);
        }
        else {
            randomSpot = Random.Range(0, patrol.movespots.Length);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetBool("isFollowing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

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
