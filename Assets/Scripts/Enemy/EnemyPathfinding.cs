using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


 /// <summary>
 /// Script for enemy pathfinding.
 /// Calculates a path either to a random spot in the scene
 /// or towards the player if they are in range.
 /// </summary>
public class EnemyPathfinding : MonoBehaviour {

    public float patrolSpeed = 600f;
    public float chaseSpeed = 1000f;
    public float nextWaypointDistance = 0.3f;

    private MoveSpots patrol;
    private int randomSpot;
    private Transform target;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool isChasing = false;
    private float speed;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        patrol = FindObjectOfType<MoveSpots>(); //find the possible spots to move to
        randomSpot = Random.Range(0, patrol.movespots.Length); //choose a random spot
        if (isChasing) {
            speed = chaseSpeed;
        }
        else {
            speed = patrolSpeed;
        }
        InvokeRepeating("UpdatePath", 0f, .5f);
    }


    public void SetIsChasing(bool b) {
        isChasing = b;
    }

     /// <summary>
     /// This function calculats a new path if the enemy
     /// has reached the end of the previous path.
     /// </summary>
    void UpdatePath() {
        //Calculate path to random spot
        if (seeker.IsDone() && !isChasing) {
            seeker.StartPath(rb.position, patrol.movespots[randomSpot].position, OnPathComplete);
        }
        //Calculate path to player
        else if (seeker.IsDone() && isChasing) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// This function creates and applies a force to push the enemy along
    /// the calculated path.
    /// </summary>
    void FixedUpdate() {

        if (isChasing) {
            speed = chaseSpeed;
        }
        else {
            speed = patrolSpeed;
        }

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            if (!isChasing) {
                NewRandomSpot();
                currentWaypoint = 0;
            }
            return;
        }
        else {
            reachedEndOfPath = false;
        }

        //Get direction of path and calculate force
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Add force to rigidbody
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

    }

    /// <summary>
    /// This function sets a new random spot from the list of move spots
    /// </summary>
    void NewRandomSpot() {
        randomSpot = Random.Range(0, patrol.movespots.Length);
    }
}
