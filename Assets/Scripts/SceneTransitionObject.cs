using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionObject : MonoBehaviour
{
    public GameObject player;
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
        if (Vector2.Distance(player.transform.position, transitionObject.transform.position) < distance
            && Input.GetButton("Fire1")) {
            ChangeScene();
        }
    }

    private void ChangeScene() {
        if (sceneLoader.GetComponent<SceneLoader>().GetCurrentScene() == secretBaseIndex) {
            //if (SceneManager.GetActiveScene().name == "SecretBase") { 
            sceneLoader.GetComponent<SceneLoader>().LoadNextScene(false);
        }
        else {
            sceneLoader.GetComponent<SceneLoader>().LoadNextScene(true);
        }
    }
}
