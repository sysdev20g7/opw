using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class NPC {
    private float _zPosition = 0; // Game is in 2D, z axis usually not used
    private static int POLICE = 1;
    private static int ZOMBIE = 2;
    [SerializeField] private int _npcType;  // 1 == police, 2 == zombie
    [SerializeField] private Vector2 xyPosition;
    [SerializeField] private int sceneIndex;
    public bool valid { get; }
    private static string _SPRITE_PREFIX_POLICE = "police";
    private static string _SPRITE_PREFIX_ZOMBIE = "goblin";
    public int health { set; get; }

    public NPC (GameObject gameObject, int associatedScene) {
        this.sceneIndex = associatedScene;
        this.xyPosition = GetPosInGameObj(gameObject);
        string spriteName = gameObject.GetComponent<SpriteRenderer>().sprite.name;
        if (spriteName.StartsWith(_SPRITE_PREFIX_POLICE)) {
            this._npcType = POLICE;
            valid = true;
        } else if (spriteName.StartsWith(_SPRITE_PREFIX_ZOMBIE)) {
            this._npcType = ZOMBIE;
            valid = true;
        } else {
            valid = false;
        }
    }

    /// <summary>
    ///  Function that gets or sets a Vector3 from the 2D coordinates
    ///  stored in this NPC object
    /// </summary>
    public Vector3 Position3Axis {
        get => new Vector3(xyPosition.x,xyPosition.y,_zPosition);
        set {
            xyPosition = new Vector2(value.x,value.y);

        }
    }

    public int GetScene() {
        return this.sceneIndex;
    }

    public int GetNpcType {
        get => _npcType;
    }

    public String getTypeString {
        get {
            int type = _npcType;
            if (type == ZOMBIE) {
                return "Zombie";
            } else  {
                return "Police";
            }
        }
    }

    /// <summary>
    /// This function returns a 2D (x,y) position from a game object 
    /// </summary>
    /// <param name="g">The game object to get position from</param>
    /// <returns>A 2D vector with x and y pos</returns>
    private Vector2 GetPosInGameObj(GameObject g) {
        return new Vector2(g.transform.position.x, g.transform.position.y);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
