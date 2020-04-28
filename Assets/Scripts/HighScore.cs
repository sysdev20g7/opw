using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    [SerializeField] private static int scoreIncrement = 10;
    public bool runScore = false;
    private ObjectController objCtrl;
    
    // Start is called before the first frame update
    void Start()
    {
        Helper obHelper = new Helper();
        this.objCtrl = obHelper.FindObjectControllerInScene();
        DisplayScore(
            this.objCtrl.runningGame.score,
            this.objCtrl.runningGame.highscore,
            false);

    }

    // Update is called once per frame
    void Update()
    {
        if (runScore) {
            InvokeRepeating(nameof(IncreaseScore),0,1f);
        }
        
    }

    private string ComposeText(int score, int high) {
        return  "SCORE: " + score + "\n"
                + "HIGHSCORE: " + high;
    }


    private void DisplayScore(int score, int highscore, bool record) {
        if (record) {
            this.GetComponent<Text>().text = ComposeText(score, highscore);
        }
        else {
            this.GetComponent<Text>().text =
                "NEW RECORD \n" + ComposeText(score, highscore);

        }
    }
    
    private void IncreaseScore() {
        bool record = this.objCtrl.IncrementScore(scoreIncrement);
        DisplayScore(
            this.objCtrl.runningGame.score,
            this.objCtrl.runningGame.highscore,
            record);
    }

    public int GetCurrentScore() {
        return this.objCtrl.runningGame.score;
    }

    public int GetHighScore() {
        return this.objCtrl.runningGame.highscore;
    }

    public void ResetCurrentScore() {
        this.objCtrl.ResetScore(true,false);
    }
}
