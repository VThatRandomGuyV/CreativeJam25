using System;
using System.Collections.Generic;
using UnityEngine;

using Combat;
using Levels;

public class Level : MonoBehaviour
{
    [Serializable]
    public struct LevelData
    {
        public LevelColor LevelColor;
        public Color HUDColor;
        public GameObject SpawnObject;
        // public AudioClip BackgroundMusic;
        public float SpawnCooldown;
        public int MaxEnemies;
    }

    /// <summary>
    /// SINGLETON
    /// </summary>
    public static Level Instance { get; private set; }
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
    /// SINGLETON

    [Header("Level Setup")]
    [SerializeField] private LevelColorManager _levelColorManager;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private List<LevelData> enemyData;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private LevelInfoSO levelInfo;
    public PlayerStats Player => player;
    public LevelColor currentColor;
    public DateTime levelStartTime;
    public DateTime levelEndTime;
    public TimeSpan timeSurvived;

    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        currentColor = newColor;
    }

    private void OnDestroy()
    {
        spawnManager.gameObject.SetActive(false);
        _levelColorManager.OnLevelColorChanged.RemoveAllListeners();
    }

    public LevelData GetLevelData(LevelColor levelColor)
    {
        return enemyData.Find(x => x.LevelColor == levelColor);
    }

    private void SpawnPlayer()
    {
        // Check if player already exists in the scene
        if (!player)
        {
            player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity).GetComponent<PlayerStats>();
            // Start level timer
            levelStartTime = DateTime.Now;
            return;
        }

        player.transform.position = playerSpawnPoint.position;
        // Start level timer
        levelStartTime = DateTime.Now;

    }

    public int GetNumberOfEnemiesDefeated(LevelColor color)
    {
        return color switch
        {
            LevelColor.Red => levelInfo.redEnemiesKillCount,
            LevelColor.Blue => levelInfo.blueEnemiesKillCount,
            LevelColor.Green => levelInfo.greenEnemiesKillCount,
            _ => levelInfo.totalEnemiesKillCount,
        };
    }

    public void UpdateEnemyKilledCount(EnemyColor color)
    {
        SpawnManager.Instance.enemyCounts[(int)color] -= 1;
        SpawnManager.Instance.totalEnemyCount -= 1;

        UpgradeManager.Instance.enemiesKilled += 1;
        UpgradeManager.Instance.enemiesKilledText.text = UpgradeManager.Instance.enemiesKilled.ToString() + " Killed";
    }

    // Start
    public void Start()
    {
        _levelColorManager.Initialize();
        //spawnManager.Initialize();

        SpawnPlayer();
    }

    // Update
    public void Update()
    {
        timeSurvived = DateTime.Now - levelStartTime;
    }
}
