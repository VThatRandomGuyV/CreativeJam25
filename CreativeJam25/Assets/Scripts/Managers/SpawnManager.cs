using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private int[] maxEnemyCounts;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform weaponProjectileContainer;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnCooldown = 3f;
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
        enemyCounts = new int[enemiesPrefabs.Length];
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
        if (totalEnemyCount < 200) { 
            int ranIndex;
            ranIndex = Random.Range(0, enemiesPrefabs.Length);

            if (enemyCounts[ranIndex] >= maxEnemyCounts[ranIndex])
            {
                lastEnemySpawnTime = spawnCooldown;
                return;
            }

            enemyCounts[ranIndex]++;

            var enemy = Instantiate(enemiesPrefabs[ranIndex], spawnPoints[ranIndex].position, Quaternion.identity, enemyContainer);
            enemy.GetComponent<Enemy>().Initialize(Level.Instance.Player);

            totalEnemyCount++;
        }
    }
}
