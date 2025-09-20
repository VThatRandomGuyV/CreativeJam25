using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Characters
{
    public class RedEnemy : Enemy
    {
        [Header(nameof(RedEnemy))]
        [SerializeField] private GameObject lavaPoolPrefab;
        [SerializeField] private float lavaPoolCooldown;
        
        private float lastLavaPoolSpawnTime;
        private bool projectileToggle;

        private readonly List<LavaPool> lavaPools = new();

        protected override void ToggleProjectiles(bool toggle)
        {
            if (projectileToggle == toggle)
            {
                return;
            }
            
            projectileToggle = toggle;
            foreach (var lavaPool in lavaPools)
            {
                lavaPool.SetActive(toggle);
            }
        }

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();
        }
    }
}