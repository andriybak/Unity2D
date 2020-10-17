using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] EnemySpawnerConfiguration SpawnConfig;

    private List<Vector2> enemyPath;

    private IEnumerator SpawnWave()
    {
        for (int ii = 0; ii < SpawnConfig.NumberOfEnemySpawns; ii++)
        {
            var enemy = SpawnConfig.EnemyObject;
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetPath(this.enemyPath);
            }

            Instantiate(enemy, Vector2.zero, Quaternion.identity);

            yield return new WaitForSeconds(SpawnConfig.SpawnDelaySeconds);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SpawnConfig == null)
        {
            Destroy(this.gameObject);
        }

        this.enemyPath = this.SpawnConfig.GetPathPoints();

        StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
