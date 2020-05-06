using System.Collections;
using UnityEngine;

/// <summary>
/// ZombieSpawner spawns or despawns <i>n</i> zombies depending
/// on if it is night or day. If it is night, it will spawn zombies, 
/// if dusk, despawn.
/// </summary>
public class ZombieSpawner : MonoBehaviour, DayListener  
{
    [SerializeField] private float spawnTime; //In seconds
    [SerializeField] private float despawnTime; //In seconds
    [SerializeField] private DayCycle dayCycle;
    private DayController dayController;

    public GameObject enemy;
    public int enemyCount;
    public int maxEnemies;

    private Coroutine spawnCoroutine;
    private Coroutine despawnCoroutine;

    private MoveSpots spawnPoints;
    private int randomSpot;

    // Start is called before the first frame update
    // Acts as a initialzing method. 
    void Start()
    {
        var temp = GameObject.FindGameObjectWithTag("DayController");
        if (temp != null) {
            dayController = temp.GetComponent<DayController>();
            dayController.subscribe(this);
            dayCycle = dayController.GetDayCycle();
            //Sets initial state
            setSpawnState();
        }
        spawnPoints = FindObjectOfType<MoveSpots>();
    }

    /// <summary>
    /// Sets internal DayCycle state to retrieved value
    /// and calls to change spawn state.
    /// </summary>
    /// <param name="dayCycle"></param>
    public void onChangeCycle(DayCycle dayCycle) {
        this.dayCycle = dayCycle;
        setSpawnState();
        Debug.Log(this + "listener: Cycle changed to " + dayCycle);
    }

    /// <summary>
    /// Decides based on what part of the day it is
    /// wether enemies should spawn or despawn.
    /// </summary>
    private void setSpawnState() {
        switch (dayCycle) {
            case DayCycle.Dawn:
                Debug.Log(this + " - Despawning " + enemy);
                if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
                despawnCoroutine = StartCoroutine(DespawnEnemies());
                break;
            case DayCycle.DayTime:
                //Do nothing.
                break;
            case DayCycle.Dusk:
                //Do nothing
                break;
            case DayCycle.NightTime:
                Debug.Log(this + " - Spawning " + enemy);
                if (despawnCoroutine != null) StopCoroutine(despawnCoroutine);
                spawnCoroutine = StartCoroutine(SpawnEnemies());
                break;
        }
    }

    /// <summary>
    /// Spawns new enemy GameObjects to the max enemy count.
    /// </summary>
    /// <returns>new WaitForSeconds(respawntime)</returns>
    private IEnumerator SpawnEnemies() {
        enemyCount = GameObject.FindGameObjectsWithTag(enemy.tag).Length;
        yield return new WaitForSeconds(1f);
        if (enemyCount <= maxEnemies) {
            while (enemyCount < maxEnemies) {
                randomSpot = Random.Range(0, spawnPoints.movespots.Length);
                enemyCount++;
                Instantiate(enemy, spawnPoints.movespots[randomSpot].position, Quaternion.identity);
                yield return new WaitForSeconds(spawnTime);
            }
        }

    }

    /// <summary>
    /// Despawns enemy GameObjects if any instances of the enemy present in scene.
    /// </summary>
    private IEnumerator DespawnEnemies() {
        var enemyList = GameObject.FindGameObjectsWithTag(enemy.tag) ;
        if (enemyList.Length > 0) {
            foreach (GameObject enemy in enemyList) {
                yield return new WaitForSeconds(despawnTime);
                enemyCount--;
                Destroy(enemy);
            }
        }
    }


    /// <summary>
    /// Unsubscribes this listener from DayController
    /// and stops all running coroutines.
    /// </summary>
    private void OnDisable() {
        if (dayController != null) {
            dayController.unsubscribe(this);
        }
        this.StopAllCoroutines();
    }
}
