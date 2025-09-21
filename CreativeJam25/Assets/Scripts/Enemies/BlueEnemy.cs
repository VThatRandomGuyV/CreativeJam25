using NUnit.Framework;
using UnityEngine;

namespace Characters
{
    public class BlueEnemy : Enemy
    {
        [Header(nameof(BlueEnemy))]
        [SerializeField] private GameObject miniBluePrefab;
        [SerializeField] private Transform minionsContainer;
        [SerializeField] private float minionsCooldown = 20f;
        [SerializeField] private int minionsMaxCount = 3;
        
        private float lastMiniBlueSpawnTime;
        private bool spawnToggle;

        private readonly Vector2[] minionSpawnPositionsOffset = new Vector2[]
        {
            new(1, 0),
            new(-1, 0),
            new(0, 1)
        };

        protected override void ToggleProjectiles(bool toggle)
        {
            if (spawnToggle == toggle)
            {
                return;
            }

            spawnToggle = toggle;
        }

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();
            
            SpawnMinions();
        }

        private void SpawnMinions()
        {
            if (!miniBluePrefab)
            {
                Debug.LogError("mini blue prefab not set.");
                return;
            }

            if (!(Time.time - lastMiniBlueSpawnTime > minionsCooldown))
            {
                return;
            }

            lastMiniBlueSpawnTime = Time.time;

            // Spawn mini blueys around the blue enemy in a triangle pattern. If the position is occupied, spawn on top of the blue enemy.
            for (int i = 0; i < minionsMaxCount; i++)
            {
                Vector2 spawnPosition = (Vector2)transform.position + minionSpawnPositionsOffset[i];

                if (Physics2D.OverlapCircle(spawnPosition, 0.1f))
                {
                    spawnPosition = transform.position;
                }

                var bluey = Instantiate(miniBluePrefab, spawnPosition, transform.rotation);
                bluey.transform.SetParent(minionsContainer);
                bluey.GetComponent<RedEnemy>().Initialize(Level.Instance.Player);
            }
        }
    }
}