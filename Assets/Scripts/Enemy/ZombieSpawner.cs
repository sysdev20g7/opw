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
        int length = GameObject.FindGameObjectsWithTag("Zombie").Length;
        yield return new WaitForSeconds(1f);
        if (length <= maxEnemies) {
            enemyCount = length;
            while (enemyCount < maxEnemies) {
                randomSpot = Random.Range(0, spawnPoints.movespots.Length);
                Instantiate(enemy, spawnPoints.movespots[randomSpot].position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                enemyCount += 1;
            }
        }
    }
}
