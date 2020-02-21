using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour {

    private MoveSpots patrol;
    private int randomSpot;
    public Transform target;
    public float patrolSpeed = 600f;
    public float chaseSpeed = 1000f;


    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool needNewSpot = false;
    bool isChasing = false;
    float speed;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        patrol = GameObject.FindObjectOfType<MoveSpots>();
        randomSpot = Random.Range(0, patrol.movespots.Length);
        if (isChasing) {
            speed = chaseSpeed;
        }
        else {
            speed = patrolSpeed;
        }
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    public void DoSomething(bool b) {
        isChasing = b;

        Start();
    }

    void UpdatePath() {
        if (seeker.IsDone() && !isChasing) {
            seeker.StartPath(rb.position, patrol.movespots[randomSpot].position, OnPathComplete);
        }
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

    // Update is called once per frame
    void FixedUpdate() {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            if (!isChasing) {
                needNewSpot = true;
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

        //Alternative method of moving enemy towards the target
        //rb.position = Vector2.MoveTowards(rb.position, path.vectorPath[currentWaypoint], speed * Time.deltaTime);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }

        if (needNewSpot) {
            needNewSpot = false;
            randomSpot = Random.Range(0, patrol.movespots.Length);
        }
    }
}
