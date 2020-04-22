using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {

    [SerializeField]
    private GameObject objectParentHealthBar; //HealthBar
    [SerializeField]
    private GameObject objectPlayer;
    [SerializeField]
    private LinkedList<GameObject> healthBar;
    private bool heartsInit = false;
    [SerializeField]
    private int currentHeartsLevel; // 0-10 internal level
    private enum HeartState {
       Full,
       Half,
       Empty,
    } 
    
    private Vector3 heartPosCurrent;
    [SerializeField]
    private Sprite spriteHeartFull, spriteHeartHalf, spriteHeartEmpty;
    
    // DO NOT TOUCH!!!
    public float offset = 34;
    public float startX = -8;
    public float startY = 32;
    public GameObject heartPrefab;
    
    public static int S_MAX_HEALTH = 10; // 1-MAX_LEVEL (usually 10)


    /// <summary>
    ///  The constructor
    /// </summary>
    public StatusBar() {
       this.healthBar = new LinkedList<GameObject>();
       this.heartPosCurrent = new Vector3(0,0,0);
    }
    
    /// <summary>
    /// This is the constructor
    /// </summary>
    void Start()
    {
        // For testing, use public methods
        InitHearts(S_MAX_HEALTH); 
        FillHearts(10); 
        DrainHearts(15);
        FillHearts(5); 
        FillHearts(10);
        this.objectPlayer = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
    }
    
    /// <summary>
    /// This function initializes the bar for the scene with empty hearts.
    /// It also adds the hearts to healthBar list for use with other functions
    /// </summary>
    /// <param name="count">Amount of hearts to display in the scene</param>
    private void InitHearts(int count) {
        this.heartPosCurrent.x += startX;
        this.heartPosCurrent.y += startY;
        if (this.heartsInit == false) {
            for (int i = 0; i < count; i++) {
                this.healthBar.AddLast(CreateHeart( "HeartLevel" + i));
            }

            if (this.healthBar.Count != 0) {
                this.heartsInit = true;
                this.currentHeartsLevel = 0;
                Debug.Log("Init health bar");
            }
        }
    }
    /// <summary>
    /// This function spawns a new heart (GameObject) in the scene, positions
    /// it and sets the heart to a empty state. 
    /// </summary>
    /// <param name="name">The name to give the GameObject</param>
    /// <returns>The newly created heart Game Object</returns>
    private GameObject CreateHeart(string name) {
            Vector3 pos = new Vector3(heartPosCurrent.x + offset, heartPosCurrent.y, 0);
            GameObject heart = (GameObject)Instantiate(heartPrefab, pos, Quaternion.identity);
            heart.name = name;
            switchHeartSprite(heart,HeartState.Empty);
            heart.transform.position = pos;
            heart.transform.parent = this.gameObject.transform;
            this.heartPosCurrent = pos;
            return heart;
    }
        


    /// <summary>
    /// Checks if the health level is valid; a positive number and also
    /// smaller than the S_MAX_HEALTH limit
    /// </summary>
    /// <param name="input">The input number to check</param>
    /// <returns>true if valid, otherwise false</returns>
    private bool validateLevel(int input) {
        if ((input >= 0) && (input <= S_MAX_HEALTH)) {
            return true;
        }
        else {
            return false;
        }
    }


    /// <summary>
    /// Switches the sprite for the selected heart to the given state
    /// </summary>
    /// <param name="heart">GameObject to switch sprite on</param>
    /// <param name="state">The state; either full;half;empty</param>
    private void switchHeartSprite(GameObject heart, HeartState state) {
        try {
            Image currentSprite = heart.GetComponent<Image>();
            switch (state) {
               case HeartState.Full:
                   if (currentSprite != null && currentSprite.sprite != spriteHeartFull) {
                       currentSprite.sprite = spriteHeartFull;
                   }
                   break;
               
               case HeartState.Half:
                   if (currentSprite != null && currentSprite.sprite != spriteHeartHalf) {
                       currentSprite.sprite = spriteHeartHalf;
                   }

                   break;
               
               case HeartState.Empty:
                   if (currentSprite != null && currentSprite.sprite != spriteHeartEmpty) {
                       currentSprite.sprite = spriteHeartEmpty;
                   }

                   break;
              
               default:
                   Debug.Log("Invalid state entered");
                   break;
            }
        }
        catch (Exception e) {
            Console.WriteLine("Unable to update sprite");
            throw;
        }
    }

    /// <summary>
    /// Fill the health level with the specified count of hearts
    /// </summary>
    /// <param name="count">Count of hearts</param>
    private void FillHearts (int count) {
        if (!(heartsInit)) return;
        
        int target = currentHeartsLevel + count;
        if (target > currentHeartsLevel) {
            if (!(validateLevel(target))) {
                target = S_MAX_HEALTH;
            }
            for (int i = currentHeartsLevel; i < target; i++) {
                switchHeartSprite(healthBar.ElementAt(i),HeartState.Full);
            }
            currentHeartsLevel = target;
        }
    }

    /// <summary>
    /// Drain the health level with the specified count of hearts
    /// </summary>
    /// <param name="count">Count of hearts</param>
    private void DrainHearts(int count) {
        if (!(heartsInit)) return;
        
        int target = currentHeartsLevel - count;
        if (target < currentHeartsLevel) {
            if ((!validateLevel(target))) {
                target = 0;
            }
            for (int i = currentHeartsLevel-1; i >= target; i--) {
                    switchHeartSprite(healthBar.ElementAt(i),HeartState.Empty);
            }
            currentHeartsLevel = target;
        }
    }
    
    //maybe remove this
    public void IncreaseHealth(int count) {
        // increase
        FillHearts(count);
    }
    
    
    // maybe remove this ? now it returns a bool is you are
    // dead or alive
    public bool DecreaseHealth(int count) {
        bool alive = true;
        DrainHearts(count);
        if (currentHeartsLevel == 0) {
            alive = false;
        }
        return alive;
    }

    /// <summary>
    /// Set the health level to a absolute value
    /// </summary>
    /// <param name="level">The heart level (between 0 and S_MAX_HEALTH)</param>
    public void setHealthLevel(int level) {
        if (currentHeartsLevel != level) {
            if (currentHeartsLevel > level) {
                DecreaseHealth(currentHeartsLevel-level);
            }
            else if (currentHeartsLevel < level) {
                IncreaseHealth(level-currentHeartsLevel);
            }
        }
    }
}
