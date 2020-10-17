using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerConfig", menuName = "EnemySpawnerConfig")]
public class EnemySpawnerConfiguration : ScriptableObject
{
    [SerializeField] public GameObject PathWithPointList;
    [SerializeField] public GameObject EnemyObject;
    [SerializeField] public float SpawnDelaySeconds = 1f;
    [SerializeField] public int NumberOfEnemySpawns = 5;

    public List<Vector2> GetPathPoints()
    {
        var resultPoints = new List<Vector2>();
        foreach (Transform childPoint in PathWithPointList.transform)
        {
            resultPoints.Add(childPoint.position);
        }

        return resultPoints;
    }

    public GameObject GetEnemyObject()
    {
        return this.EnemyObject;
    }

    public void SetSpawnDelaySeconds(float seconds)
    {
        this.SpawnDelaySeconds = seconds;
    }

    public float GetSpawnDelaySeconds()
    {
        return this.SpawnDelaySeconds;
    }

    public void SetNumberOfEnemySpawns(int numberOfEnemies)
    {
        this.NumberOfEnemySpawns = numberOfEnemies;
    }

    public int GetNumberOfEnemySpawns()
    {
        return this.NumberOfEnemySpawns;
    }
}
