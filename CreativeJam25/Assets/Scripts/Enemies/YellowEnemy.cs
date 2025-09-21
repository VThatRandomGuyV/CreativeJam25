using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class YellowEnemy : Enemy
    {
        [Header(nameof(YellowEnemy))]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileCooldown = 3f;
        [SerializeField] private float projectileDamage = 10f;
        [SerializeField] private float projectileSpeed = 800f;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private Transform weaponProjectilContainer;

        private Core.Timer timer;
        private bool projectileToggle;
        private Transform weaponProjectileContainer;

        private readonly List<Bullet> lazers = new();

        public void Start()
        {
            if (!weaponProjectilContainer)
            {
                Debug.Log("weapon container is null");
            }
        }

        protected override void MoveTowardsPlayer()
        {
            base.MoveTowardsPlayer();

            var trajectory = playerPosition - (Vector2)transform.position;
            trajectory.Normalize();

            // Flip the spawn point of the projectile
            shootingPoint.transform.position = transform.position + (Vector3)trajectory * 0.5f;
            shootingPoint.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg);

            SpawnLazerProjectile();
        }

        private void SpawnLazerProjectile()
        {
            if (timer == null || timer.time <= 0f)
            {
                // Reset timer
                timer = new Core.Timer(projectileCooldown);

                var trajectoryVector = playerPosition - (Vector2)shootingPoint.transform.position;
                // if the vector maginitude is too small, magnify it
                if (trajectoryVector.magnitude < 1.0f)
                {
                    trajectoryVector *= 10f;
                }

                trajectoryVector.Normalize();

                Quaternion prefabRotation = Quaternion.Euler( 0, 0, 
                                        Mathf.Atan2 ( trajectoryVector.y, trajectoryVector.x ) * Mathf.Rad2Deg );
                
                var bullet = Instantiate(projectilePrefab, shootingPoint.transform.position, prefabRotation, weaponProjectileContainer);
                var bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.Initialize(trajectoryVector, "YellowEnemy", projectileSpeed, projectileDamage);
                lazers.Add(bulletComponent);
            }
            timer.Tick(Time.fixedDeltaTime);
        }
    }
}