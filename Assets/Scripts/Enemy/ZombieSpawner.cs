using System.Collections;
using UnityEngine;

/// <summary>
/// ZombieSpawner spawns or despawns <i>n</i> zombies depending
/// on if it is night or day. If it is night, it will spawn zombies, 
/// if day, despawn.
/// </summary>
public class ZombieSpawner : MonoBehaviour, DayListener  
{
    public GameObject enemy;
    public int enemyCount;
    public int maxEnemies;

    [SerializeField]
    private float respawnTime; //In seconds
    [SerializeField]
    private float despawnTime; //In seconds

    private MoveSpots spawnPoints;
    private int randomSpot;

    private DayCycle dayCycle;
    private DayController dayController;

    // Start is called before the first frame update
    // Acts as a initialzing method. 
    void Start()
    {
        spawnPoints = FindObjectOfType<MoveSpots>();
        var temp = GameObject.FindGameObjectWithTag("DayController");
        if (temp != null) {
            dayController = temp.GetComponent<DayController>();
            dayController.addListener(this);
            dayCycle = dayController.GetDayCycle();
        }
    }

    /// <summary>
    /// Sets internal DayCycle state to retrieved value
    /// and calls to change spawn state.
    /// </summary>
    /// <param name="dayCycle"></param>
    public void onChangeCycle(DayCycle dayCycle) {
        this.dayCycle = dayCycle;
        changeSpawnState();
        Debug.Log(this + "-Listener: Cycle changed to " + dayCycle);
    }

    /// <summary>
    /// Decides based on what part of the day it is
    /// wether Zombies should spawn or despawn.
    /// </summary>
    private void changeSpawnState() {
        switch (dayCycle) {
            case DayCycle.Dawn:
                Debug.Log(this + " - Despawning Zombies");
                DespawnEnemies();
                break;
            case DayCycle.DayTime:
                //Do nothing.
                break;
            case DayCycle.Dusk:
                //Do nothing
                break;
            case DayCycle.NightTime:
                Debug.Log(this + " - Spawning Zombies");
                StartCoroutine(EnemySpawn());
                break;
        }
    }

    /// <summary>
    /// Spawns new Zombie GameObjects 
    /// </summary>
    /// <returns>new WaitForSeconds</returns>
    private IEnumerator EnemySpawn() {
        enemyCount = GameObject.FindGameObjectsWithTag("Zombie").Length;
        yield return new WaitForSeconds(1f);
        if (enemyCount <= maxEnemies) {
            while (enemyCount < maxEnemies) {
                randomSpot = Random.Range(0, spawnPoints.movespots.Length);
                Instantiate(enemy, spawnPoints.movespots[randomSpot].position, Quaternion.identity);
                yield return new WaitForSeconds(respawnTime);
                enemyCount += 1;
            }
        }
    }

    /// <summary>
    /// Despawns zombie GameObjects if any zombies present in scene.
    /// </summary>
    private IEnumerator DespawnEnemies() {
        var zombieList = GameObject.FindGameObjectsWithTag("Zombie");
        if (zombieList.Length > 0) {
            foreach (GameObject zombie in zombieList) {
                yield return new WaitForSeconds(despawnTime);
                Destroy(zombie);
            }
        }
    }
}
