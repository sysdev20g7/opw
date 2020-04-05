using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawner;
    public int enemyCount;
    public int maxEnemies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn() {
        int length = GameObject.FindGameObjectsWithTag("Police").Length;
        yield return new WaitForSeconds(1f);
        if (length <= maxEnemies) {
            enemyCount = length;
            while (enemyCount < maxEnemies) {
                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                enemyCount += 1;
            }
    }
}
}
