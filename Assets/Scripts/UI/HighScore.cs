using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class updates high score data, and displays them in-game.
/// Values are stored in GameData, and calculated in Object Controller
/// </summary>
public class HighScore : MonoBehaviour {
    // runScore is used by other classes to start/stop the score counter
    public bool runScore; 
    public int scoreIncrement = 10;
    public float incrementInterval = 1f;
    
    [SerializeField]
    private bool newHighScore = false;
    private int recordIncrements = 0;
    private ObjectController objCtrl;
    private GameObject scoreDigits; //Line0 S, Line1 H
    
    // Start is called before the first frame update
    void Start()
    {
        // find obj controller and score digit object
        Helper obHelper = new Helper();
        this.objCtrl = obHelper.FindObjectControllerInScene();
        this.scoreDigits = GameObject.FindWithTag("ScoreDigitUI");
        DisplayScore(
            this.objCtrl.runningGame.score,
            this.objCtrl.runningGame.highscore,
            newHighScore);

        //Only needed to call once, as unity repeates TickInterval automatically
        InvokeRepeating(nameof(TickInterval),0,incrementInterval);
    }

    /// <summary>
    /// Formats the on screen visible text during gameplay.
    /// Overwrites the text in the prefab
    /// </summary>
    /// <returns>formatted string for display</returns>
    private string ComposeText() {
        return  "SCORE" + "\n"
                + "HIGH SCORE";
    }

    /// <summary>
    /// Responsible for presenting data to the display in game
    /// </summary>
    /// <param name="score">score value to display</param>
    /// <param name="highscore">highscore value to display</param>
    /// <param name="record">display special message if new high score is true</param>
    private void DisplayScore(int score, int highscore, bool record) {
        if (!(record)) {
            this.GetComponent<TMP_Text>().text = ComposeText();
            this.scoreDigits.GetComponent<TMP_Text>().text = score + "\n" + highscore;
        }
        else {
            string highScoreString = "";
            if (this.recordIncrements < 5) {
                highScoreString = "\nNEW RECORD";
                
            }
            this.GetComponent<TMP_Text>().text =
                 ComposeText() 
                 + highScoreString;
            
            this.scoreDigits.GetComponent<TMP_Text>().text = score + "\n" + highscore;
        }
    }

    /// <summary>
    /// Runs during the interval, increments the score if runScore is
    /// enabled
    /// </summary>
    private void TickInterval() {
        if (runScore) {
            IncreaseScore();
        }
    }
    
    /// <summary>
    ///  Calls object controller to calculate new score for each increment;
    /// passes on return values for score, high score, and new record to the
    /// DisplayScore function
    /// </summary>
    private void IncreaseScore() {
        newHighScore = this.objCtrl.IncrementScore(scoreIncrement);
        DisplayScore(
            this.objCtrl.runningGame.score,
            this.objCtrl.runningGame.highscore,
            newHighScore);
        if (newHighScore) {
            this.recordIncrements++;
        }
    }
}
