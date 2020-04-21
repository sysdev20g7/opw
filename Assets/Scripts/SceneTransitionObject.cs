using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionObject : MonoBehaviour
{
    private GameObject player;
    public GameObject transitionObject;
    public GameObject sceneLoader;
    public float distance;
    public int secretBaseIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector2.Distance(player.transform.position, transitionObject.transform.position) < distance
            && Input.GetButton("Action")) {
            ChangeScene();
        }
    }

    private void ChangeScene() {
        if (sceneLoader.GetComponent<SceneLoader>().GetCurrentScene() == secretBaseIndex) {
            sceneLoader.GetComponent<SceneLoader>().LoadNextScene(false);
        }
        else {
            sceneLoader.GetComponent<SceneLoader>().LoadNextScene(true);
        }
    }
}
