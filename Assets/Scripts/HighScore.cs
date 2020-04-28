using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    [SerializeField] private static int scoreIncrement = 10;
    public bool runScore = false;
    [SerializeField]
    private bool record = false;
    private ObjectController objCtrl;
    
    // Start is called before the first frame update
    void Start()
    {
        Helper obHelper = new Helper();
        this.objCtrl = obHelper.FindObjectControllerInScene();
        DisplayScore(
            this.objCtrl.runningGame.score,
            this.objCtrl.runningGame.highscore,
            record);

        InvokeRepeating(nameof(TickInterval),0,1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string ComposeText(int score, int high) {
        return  "SCORE" + "\t\t\t\t\t" + score + "\n"
                + "HIGH SCORE " + "\t\t" + high;
    }


    private void DisplayScore(int score, int highscore, bool record) {
        if (!(record)) {
            this.GetComponent<Text>().text = ComposeText(score, highscore);
        }
        else {
            this.GetComponent<Text>().text =
                 ComposeText(score, highscore) 
                 + "\n NEW RECORD";

        }
    }

    private void TickInterval() {
        if (runScore) {
            IncreaseScore();
        }
    }
    private void IncreaseScore() {
        record = this.objCtrl.IncrementScore(scoreIncrement);
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
