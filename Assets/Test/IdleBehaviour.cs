using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IdleBehaviour : StateMachineBehaviour {

    //private MoveSpots patrol;
    //public float speed;
    //private int randomSpot;
    private Transform player;
    public float range;

    //public float nextWaypointDistance = 3f;

    //Path path;
    //int currentWaypoint = 0;
    //bool reachedEndOfPath = false;

    //Seeker seeker;
    //Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //seeker = animator.GetComponent<Seeker>();
        //rb = animator.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //patrol = FindObjectOfType<MoveSpots>();
        //randomSpot = Random.Range(0, patrol.movespots.Length);
        //seeker.StartPath(rb.position, patrol.movespots[randomSpot].position, OnPathComplete);
        animator.GetComponent<EnemyPathfindingWaypoint>().DoSomething();
    }

    //void UpdatePath() {
    //    if (seeker.IsDone()) {
    //        seeker.StartPath(rb.position, patrol.movespots[randomSpot].position, OnPathComplete);
    //    }
    //}
    //void OnPathComplete(Path p) {
    //    if (!p.error) {
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //if (path == null)
        //    return;

        //if (currentWaypoint >= path.vectorPath.Count) {
        //    reachedEndOfPath = true;
        //    return;
        //}
        //else {
        //    reachedEndOfPath = false;
        //}

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;

        //rb.AddForce(force);

        //float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //if (distance < nextWaypointDistance) {
        //    currentWaypoint++;
        //}




        //if (Vector2.Distance(animator.transform.position, patrol.movespots[randomSpot].position) > 0.2f) {
        //animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrol.movespots[randomSpot].position, speed * Time.deltaTime);
        //}
        //else {
        //    randomSpot = Random.Range(0, patrol.movespots.Length);
        //}

        if (Vector2.Distance(player.position, animator.transform.position) <= range) {
            animator.SetBool("isFollowing", true);
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
