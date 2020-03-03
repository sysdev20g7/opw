using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator animation;

    public float animationDuration = 1f; 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            LoadNextScene(true);
        }
        else if (Input.GetMouseButtonDown(1)) {
            LoadNextScene(false);
        }
    }

    public void LoadNextScene( bool next) {

        if (next) {
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else {
            
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex - 1));
        }
    }

    IEnumerator LoadScene(int sceneIndex) {
        animation.SetTrigger("Begin");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
}
