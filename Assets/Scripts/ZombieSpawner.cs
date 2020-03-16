using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject enemy;
    public int enemyCount;
    public int maxEnemies;

    private MoveSpots spawnPoints;
    private int randomSpot;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectOfType<MoveSpots>();
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn() {
        while (enemyCount < maxEnemies) {
            randomSpot = Random.Range(0, spawnPoints.movespots.Length);
            Instantiate(enemy, spawnPoints.movespots[randomSpot].position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
