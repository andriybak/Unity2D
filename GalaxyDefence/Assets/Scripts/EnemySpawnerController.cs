using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] List<EnemySpawnerConfiguration> SpawnConfigList;
    [SerializeField] List<GameObject> PowerUps;

    private IEnumerator SpawnWave(EnemySpawnerConfiguration spawnConfig)
    {
        var enemyPowerUpIndex = Random.Range(0, spawnConfig.NumberOfEnemySpawns);

        for (int ii = 0; ii < spawnConfig.NumberOfEnemySpawns; ii++)
        {
            var enemy = spawnConfig.EnemyObject;
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetPath(spawnConfig.GetPathPoints());
            }

            var enemyObject = Instantiate(enemy, Vector2.zero, Quaternion.identity);
            if (ii == enemyPowerUpIndex)
            {
                var randomPowerUp = Random.Range(0, this.PowerUps.Count);
                enemy.GetComponent<EnemyController>().powerUp = this.PowerUps[randomPowerUp];
            }

            yield return new WaitForSeconds(spawnConfig.SpawnDelaySeconds);
        }
    }

    private IEnumerator StartSpawnLoop()
    {
        while (true)
        {
            foreach (var wave in this.SpawnConfigList)
            {
                StartCoroutine(SpawnWave(wave));
                yield return new WaitForSeconds(1);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.SpawnConfigList == null || this.PowerUps == null)
        {
            Destroy(this.gameObject);
            return;
        }

        StartCoroutine(StartSpawnLoop());
    }
}
