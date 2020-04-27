using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {
    [SerializeField]
    private int currentScore;
    [SerializeField]
    private int highScore;
    [SerializeField] private static int scoreIncrement = 10;
    public bool runScore = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Helper obHelper = new Helper();
        ObjectController objCtrl = obHelper.FindObjectControllerInScene();
        if (!(objCtrl is null)) {
            
        }

        this.currentScore = objCtrl.runningGame.score;
        this.highScore = objCtrl.runningGame.highscore;

    }

    // Update is called once per frame
    void Update()
    {
        if (runScore) {
            InvokeRepeating(nameof(IncreaseScore),0,1f);
        }
        
    }


    private void IncreaseScore() {
        highScore += scoreIncrement;
    }

    
    
}
