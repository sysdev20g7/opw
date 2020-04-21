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
    private int currentHeartsLevel; // 0-9 internal level
    private enum HeartState {
       Full,
       Half,
       Empty,
    } 
    
    private Vector3 heartPosCurrent;
    [SerializeField]
    private Sprite spriteHeartFull;
    [SerializeField]
    private Sprite spriteHeartHalf;
    [SerializeField]
    private Sprite spriteHeartEmpty;
    
    public float offset = 34;
    public float startX = -8;
    public float startY = 32;
    public GameObject heartPrefab;
    
    public static int S_MAX_HEALTH = 10; // 1-10 external level


    public StatusBar() {
       this.healthBar = new LinkedList<GameObject>();
       this.heartPosCurrent = new Vector3(0,0,0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
       InitHearts(S_MAX_HEALTH);
       FillHearts(10);
       DrainHearts(9);
       DrainHearts(1);
       FillHearts(1);
       this.objectPlayer = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
    }
    
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
        
    private void InitHearts(int count) {
        this.heartPosCurrent.x += startX;
        this.heartPosCurrent.y += startY;
        if (this.heartsInit == false) {
            this.heartsInit = true;
            
            if (count <= 0) {
            }
            else {
                for (int i = 0; i < count; i++) {
                    this.healthBar.AddLast(CreateHeart( "HeartLevel" + i));
                }

                this.currentHeartsLevel = 0;
                Debug.Log("Init health bar");
            }
        }
    }


    private bool validateLevel(int input) {
        if ((input >= 0) && (input <= S_MAX_HEALTH)) {
            return true;
        }
        else {
            return false;
        }
    }

    private int convertToZeroIndex(int input) {
        return input - 1;
    }

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

    private void FillHearts (int count) {
        int target = currentHeartsLevel + count;
        if (target > currentHeartsLevel) {
            if (validateLevel(target)) {
                    for (int i = convertToZeroIndex(currentHeartsLevel); i < convertToZeroIndex(target); i++) {
                        switchHeartSprite(healthBar.ElementAt(i),HeartState.Full);
                    }
                    currentHeartsLevel = target;
            }
        }
    }

    private void DrainHearts(int count) {
        int target = currentHeartsLevel - count;
        if (target < currentHeartsLevel) {
            if (validateLevel(target)) {
                    for (int i = convertToZeroIndex(currentHeartsLevel); i > convertToZeroIndex(target); i--) {
                        switchHeartSprite(healthBar.ElementAt(i),HeartState.Empty);
                    }
                    currentHeartsLevel = target;
            }
        }
    }
    
    public void IncreaseHealth(int count) {
        // increase
        FillHearts(count);
    }
    
    
    public void DecreaseHealth(int count) {
        DrainHearts(count);
    }

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
