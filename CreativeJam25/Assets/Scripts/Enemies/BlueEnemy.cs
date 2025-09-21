using UnityEngine;

namespace Characters
{
    public class BlueEnemy : Enemy
    {
        [Header(nameof(BlueEnemy))]
        [SerializeField] private GameObject miniBluePrefab;
        [SerializeField] private float miniBlueCooldown;
        
        private float lastMiniBlueSpawnTime;
        private bool spawnToggle;

        //private readonly List<BlueMini> minions = new();

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
            
            //SpawnLavaPool();
        }

        //private void SpawnLavaPool()
        //{
        //    if (!miniBluePrefab)
        //    {
        //        Debug.LogError("mini blue prefab not set.");
        //        return;
        //    }

        //    if (!(Time.time - lastMiniBlueSpawnTime > miniBlueCooldown))
        //    {
        //        return;
        //    }

        //    lastMiniBlueSpawnTime = Time.time;
        //    var minion = Instantiate(miniBluePrefab, transform.position, transform.rotation);
        //    minion.transform.SetParent(weaponProjectileContainer);
        //    minions.Add(lavaPool.GetComponent<LavaPool>());
        //}
    }
}