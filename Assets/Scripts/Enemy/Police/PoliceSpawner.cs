using System.Collections;
using UnityEngine;

/// <summary>
/// PoliceSpawner spawns <i>n</i> police after a 
/// variable amount of time.
/// </summary>
public class PoliceSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawner;
    public int enemyCount;
    public int maxEnemies;
    public float timeBetweenSpawn = 0.4f; //in seconds
    public float timeUntilSpawn = 60f; //in seconds

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    /// <summary>
    /// Spawns new police GameObjects up to the max count.
    /// </summary>
    /// <returns>new WaitForSeconds(timeBetweenSpawn)</returns>
    IEnumerator EnemySpawn() {
        yield return new WaitForSeconds(timeUntilSpawn);
        int length = GameObject.FindGameObjectsWithTag("Police").Length;
        yield return new WaitForSeconds(1f);
        if (length <= maxEnemies) {
            enemyCount = length;
            while (enemyCount < maxEnemies) {
                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(timeBetweenSpawn);
                enemyCount += 1;
            }
    }
}
}
