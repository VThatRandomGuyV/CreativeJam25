using System.Collections;
using System.Collections.Generic;
using Characters;
using Levels;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private int[] max;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform weaponProjectileContainer;
    [SerializeField] private Transform[] spawnPoints;
    
    private float lastEnemySpawnTime;
    private int totalEnemyCount;
    private int[] enemyCounts;

    public void Start()
    {
        enemyCounts = new int[enemiesPrefabs.Length];
    }

    private void FixedUpdate()
    {
        if (lastEnemySpawnTime > 3f)
        {
            SpawnEnemy();
            lastEnemySpawnTime = 0;
        }
        else
        {
            lastEnemySpawnTime += Time.fixedDeltaTime;
        }
    }

    private void SpawnEnemy()
    {
        if (totalEnemyCount < 50) { 
            int ranIndex;
            do
            {
                ranIndex = Random.Range(0, enemiesPrefabs.Length);
            } while (enemyCounts[ranIndex] >= max[ranIndex]);

            enemyCounts[ranIndex]++;

            var enemy = Instantiate(enemiesPrefabs[ranIndex], spawnPoints[ranIndex].position, Quaternion.identity, enemyContainer);
            enemy.GetComponent<Enemy>().Initialize(Level.Instance.Player);

            totalEnemyCount++;
        }
    }
}
