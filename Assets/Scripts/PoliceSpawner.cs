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
        if (FindObjectsOfType<Police>().Length <= maxEnemies) {
            while (enemyCount < maxEnemies) {
                Instantiate(enemy, spawner.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                enemyCount += 1;
            }
    }
}
}
