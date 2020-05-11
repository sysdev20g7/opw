using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate() {

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -20);
    }
}
