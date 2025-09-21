using Characters;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private int[] maxEnemyCounts;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform weaponProjectileContainer;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnCooldown = 3f;
    [SerializeField] private int redRate = 60;
    [SerializeField] private int blueRate = 10;
    [SerializeField] private int greenRate = 15;
    [SerializeField] private int orangeRate = 15;
    [SerializeField] private int maxEnemy = 200;

    public int totalEnemyCount;
    private float lastEnemySpawnTime;
    public int[] enemyCounts;

    public static SpawnManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        enemyCounts = new int[4];
    }

    private void FixedUpdate()
    {
        if (lastEnemySpawnTime >= spawnCooldown)
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
        if (totalEnemyCount < maxEnemy) { 
            int prefabIndex = 0;
            int weightedEnemyRNG;
            weightedEnemyRNG = Random.Range(1, 101);
            if(weightedEnemyRNG < redRate)
            {
                prefabIndex = 0;
            }else if(weightedEnemyRNG >= redRate && weightedEnemyRNG < redRate+blueRate)
            {
                prefabIndex = 1;
            }
            else if (weightedEnemyRNG >= redRate+blueRate && weightedEnemyRNG < redRate+blueRate+greenRate)
            {
                prefabIndex = 2;
            }
            else if (weightedEnemyRNG >= redRate+blueRate+greenRate && weightedEnemyRNG <= redRate + blueRate + greenRate+orangeRate)
            {
                prefabIndex = 3;
            }

            if (enemyCounts[prefabIndex] >= maxEnemyCounts[prefabIndex])
            {
                lastEnemySpawnTime = spawnCooldown;
                return;
            }

            enemyCounts[prefabIndex]++;

            var enemy = Instantiate(enemiesPrefabs[prefabIndex], spawnPoints[prefabIndex].position, Quaternion.identity, enemyContainer);
            enemy.GetComponent<Enemy>().Initialize(Level.Instance.Player);

            totalEnemyCount++;
        }
    }
}
