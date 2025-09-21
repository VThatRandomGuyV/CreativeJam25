using System;
using UnityEngine;
using UnityEngine.AI;

using System.Collections;

namespace Characters
{
    public abstract class Enemy : Character
    {
        public event Action<Enemy> OnDeath;

        [Header(nameof(Enemy))]
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected EnemyColor entityColor;

        [Header("Knockback 0.25f - 2.0f")]
        [SerializeField] private float knockbackForce;

        [Header("Shield")]
        [SerializeField] protected float shieldHP;
        [SerializeField] private bool buffed = false;
        [SerializeField] protected SpriteRenderer shieldSpriteRenderer;

        [Header("Item Drop")]
        [SerializeField] private GameObject itemDropPrefab;

        protected EnemyState currentState;
        protected PlayerStats player;
        protected float lastAttackTime;
        protected Vector2 playerPosition;
        protected Vector2 normalizedTrajectoryToPlayer;
        protected float shieldDuration = 10f;

        public Collider2D EnemyCollider => characterCollider;
        public SpriteRenderer SpriteRenderer => GetComponentInChildren<SpriteRenderer>();
        public float AttackDamage => attackDamage;

        public virtual void Initialize(PlayerStats player)
        {
            this.player = player;

            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
            }

            lastAttackTime = Time.time;
            currentState = EnemyState.Attacking;
            gameObject.tag = "Enemy";

            // Nav Mesh Agent settings
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;

            // Shield settings
            shieldSpriteRenderer.enabled = false;
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            // Check player is assigned, enemy health is above 0, and not dead
            if (!player)
            {
                Debug.LogError("Player is not active.");
                Destroy(this);
            }

            if (currentState is EnemyState.Dead)
            {
                characterRigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }

            // Enemy behavior
            gameObject.SetActive(true);
            ToggleProjectiles(true);
            playerPosition = player.transform.position;

            PlaceTilesOnTileset(new Vector3[] { transform.position }, TileManager.Instance.tileset, (int)entityColor);

            if (!IsNearPlayer())
            {
                MoveTowardsPlayer();
                return;
            }

            characterRigidbody.linearVelocity = Vector2.zero;
            TryAttackPlayer();
        }

        protected virtual void ToggleProjectiles(bool toggle)
        {

        }

        private IEnumerator ShieldDurationCoroutine()
        {
            yield return new WaitForSeconds(shieldDuration);
            buffed = false;
            shieldHP = 0f;
            shieldSpriteRenderer.enabled = false;
        }

        internal void ShieldYourself(float shieldAmount)
        {
            if (buffed)
            {
                return;
            }

            buffed = true;
            shieldHP = shieldAmount;
            shieldSpriteRenderer.enabled = true;
            StartCoroutine(ShieldDurationCoroutine());
        }

        protected virtual void MoveTowardsPlayer()
        {
            navMeshAgent.SetDestination(playerPosition);

            // Flip sprite depending on player position
            characterSpriteRenderer.flipX = playerPosition.x < transform.position.x;
        }

        protected bool IsNearPlayer()
        {
            var distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
            return distanceToPlayer <= attackRange;
        }

        protected virtual void TryAttackPlayer()
        {
            if (!IsNearPlayer() || Time.time - lastAttackTime < attackCooldown || currentState is not EnemyState.Attacking)
            {
                return;
            }

            player.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }

        public void TakeDamage(float damageTaken)
        {
            if (shieldHP > 0)
            {
                shieldHP -= damageTaken;
            }
            else
            {
                shieldSpriteRenderer.enabled = false;
                health -= damageTaken;

                // Death check
                if (health <= 0)
                {
                    Death();
                    return;
                }
            }

            // Knockback effect
            var knockbackDirection = (Vector2)transform.position - playerPosition;
            knockbackDirection.Normalize();
            transform.position += (Vector3)knockbackDirection * knockbackForce;
        }

        public void TakeDamageNoKnockback(float damageTaken)
        {
            if (shieldHP > 0)
            {
                shieldHP -= damageTaken;
            }
            else
            {
                shieldSpriteRenderer.enabled = false;
                health -= damageTaken;

                // Death check
                if (health <= 0)
                {
                    Death();
                    return;
                }
            }
        }

        public void SetEnemyState(EnemyState enemyState)
        {
            currentState = enemyState;
            ToggleProjectiles(currentState is EnemyState.Attacking);
            gameObject.SetActive(currentState is EnemyState.Attacking);
        }

        public void Death()
        {
            // Update enemy kill count logic
            Level.Instance.UpdateEnemyKilledCount(Level.Instance.currentColor);

            // Update enemy object
            characterRigidbody.freezeRotation = false;
            characterCollider.enabled = false;
            currentState = EnemyState.Dead;

            // Instantiate item drop
            if (itemDropPrefab)
            {
                Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the enemy object after a delay
            Destroy(gameObject, 2f);
            OnDeath?.Invoke(this);
        }
        
        void PlaceTilesOnTileset(Vector3[] positions, UnityEngine.Tilemaps.Tilemap tileset, int color)
        {

            for (int i = 0; i < positions.Length; i++)
            {
                tileset.SetTileFlags(tileset.WorldToCell(positions[i]), UnityEngine.Tilemaps.TileFlags.None);
                switch (color)
                {
                    case 0:
                        tileset.SetColor(tileset.WorldToCell(positions[i]), Color.gray);
                        
                        break;
                    case 1:
                        tileset.SetColor(tileset.WorldToCell(positions[i]), Color.red);
                        break;
                    case 2:
                        tileset.SetColor(tileset.WorldToCell(positions[i]), Color.blue);
                        break;
                    case 3:
                        tileset.SetColor(tileset.WorldToCell(positions[i]), Color.green);
                        break;
                    case 4:
                        tileset.SetColor(tileset.WorldToCell(positions[i]), Color.yellow);
                        break;
                }
            }
        }
    }
}