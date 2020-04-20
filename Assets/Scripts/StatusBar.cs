using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusBar : MonoBehaviour {

    private GameObject parentHealthBar; //HealthBar
    private GameObject player;
    private bool heartsInit = false;
    
    [SerializeField]
    private Vector3 heartPosCurrent;
    [SerializeField]
    private Sprite spriteHeartFull;
    [SerializeField]
    private Sprite spriteHeartHalf;
    [SerializeField]
    private Sprite spriteHeartEmpty;
    [SerializeField]
    private float offset = 32;

    public GameObject heartPrefab;

    // Start is called before the first frame update
    void Start()
    {
       this.heartPosCurrent = new Vector3(0,0,0);
       this.player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
       InitHearts(10);
    }


    private void CreateHeart( string name) {
            Vector3 pos = new Vector3(heartPosCurrent.x + offset, heartPosCurrent.y, 0);
            GameObject heart = (GameObject)Instantiate(heartPrefab, pos, Quaternion.identity);
            heart.name = name;
            heart.transform.position = pos;
            heart.transform.parent = this.gameObject.transform;
            this.heartPosCurrent = pos;
    }
        
    public void InitHearts(int count) {
        this.heartPosCurrent.x = this.heartPosCurrent.x + offset;
        if (this.heartsInit == false) {
            if (count <= 0) {
            }
            else {
                for (int i = 0; i < count-1; i++) {
                    CreateHeart( "HeartLevel" + i);
                    this.heartsInit = true;
                }

                Debug.Log("Init health bar");
            }
        }
    }
    
    
}
