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
    
    private float lastEnemySpawnTime;
    private bool isInitialized;
    private int enemyCount;
    private int[] enemyCounts;

    public void Start()
    {
        isInitialized = true;
        enemyCounts = new int[enemiesPrefabs.Length];
    }



    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }
        if (lastEnemySpawnTime > 3f)
        {
            SpawnEnemy();
            lastEnemySpawnTime = 0;
        }
        else
        {
            lastEnemySpawnTime += Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyCount < 50) { 
            int ranIndex;
            do
            {
                ranIndex = Random.Range(0, enemiesPrefabs.Length);
            } while (enemyCounts[ranIndex] >= max[ranIndex]);
            enemyCounts[ranIndex]++;
            var enemy = Instantiate(enemiesPrefabs[ranIndex]);
            enemy.GetComponent<Enemy>().Initialize(Level.Instance.Player);
            enemyCount++;
        }
    }
}
