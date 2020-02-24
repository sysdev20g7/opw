using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    // Use method for player input
    void Update() {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    //Use method for player movement
    void FixedUpdate() {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
