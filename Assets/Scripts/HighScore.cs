using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {
    [SerializeField] private static int scoreIncrement = 10;
    public bool runScore = false;
    [SerializeField]
    private bool record = false;
    private ObjectController objCtrl;
    private GameObject scoreDigits; //Line0 S, Line1 H
    
    // Start is called before the first frame update
    void Start()
    {
        Helper obHelper = new Helper();
        this.objCtrl = obHelper.FindObjectControllerInScene();
        this.scoreDigits = GameObject.FindWithTag("ScoreDigitUI");
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

    private string ComposeText() {
        return  "SCORE" + "\n"
                + "HIGH SCORE ";
    }


    private void DisplayScore(int score, int highscore, bool record) {
        if (!(record)) {
            this.GetComponent<TMP_Text>().text = ComposeText();
            this.scoreDigits.GetComponent<TMP_Text>().text = score + "\n" + highscore;
        }
        else {
            this.GetComponent<TMP_Text>().text =
                 ComposeText() 
                 + "\nNEW RECORD";
            
            this.scoreDigits.GetComponent<TMP_Text>().text = score + "\n" + highscore;
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
