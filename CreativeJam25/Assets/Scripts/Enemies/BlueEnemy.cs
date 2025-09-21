using UnityEngine;

namespace Characters
{
    public class BlueEnemy : Enemy
    {
        [Header(nameof(BlueEnemy))]
        [SerializeField] private GameObject miniBluePrefab;
        [SerializeField] private Transform minionsContainer;
        [SerializeField] private float minionsCooldown = 15f;
        [SerializeField] private int minionsMaxCount = 3;
        
        private float lastMiniBlueSpawnTime;
        private bool spawnToggle;

        protected override void ToggleProjectiles(bool toggle)
        {
            if (spawnToggle == toggle)
            {
                return;
            }

            spawnToggle = toggle;
            //foreach (var mini in minions)
            //{
            //    mini.SetActive(toggle);
            //}
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
                float angle = i * 120f;
                Vector2 spawnPosition = (Vector2)transform.position + (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.up * 0.5f);

                if (!Physics2D.OverlapCircle(spawnPosition, 0.1f))
                {
                    var bluey = Instantiate(miniBluePrefab, spawnPosition, transform.rotation);
                    bluey.transform.SetParent(minionsContainer);
                    return;
                }
            }

            var minion = Instantiate(miniBluePrefab, transform.position, transform.rotation);
            minion.transform.SetParent(minionsContainer);
        }
    }
}