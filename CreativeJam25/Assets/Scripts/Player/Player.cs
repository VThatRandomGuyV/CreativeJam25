using Characters;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerState playerState;
    private PlayerController playerController;

    private Collider2D[] enemiesInAura;

    private Collider2D[] enemiesInRange;

    private float poisonAuraTimer = 0f;
    private float slowAuraTimer = 0f;

    [Header("Void Projectile Settings")]
    [SerializeField] private int voidProjectileCountBase = 1;
    [SerializeField] private float voidProjectileSpeed = 1000f;
    [SerializeField] private float voidProjectileDamage = 5f;
    [SerializeField] private float voidProjectilRangeMod = 1.5f;
    [SerializeField] private float VPACooldown = 1f; // Void Projectile Attack Cooldown
    [SerializeField] private GameObject voidProjectilePrefab;

    [Header("Orbit ball Settings")]
    [SerializeField] private GameObject orbitBallPrefab;
    [SerializeField] private float orbitBallSpeed = 5f;
    [SerializeField] private float orbitBallDamage = 5f;
    [SerializeField] private float orbitBallRadiusBase = 1.5f;
    private List<GameObject> orbitBalls = new();

    [SerializeField] private float basePoisonDamage = 0.02f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerState = GetComponent<PlayerState>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemiesInAura = Physics2D.OverlapCircleAll(transform.position, playerStats.voidRadius, LayerMask.GetMask("Enemy"));
        if (playerStats.PoisonAuraLevel >= 0)
        {
            if (poisonAuraTimer >= 0.5f)
            {
                PoisonEnemies();
                poisonAuraTimer = 0f;
            }
        }
        if (playerStats.SlowAuraLevel >= 0)
        {
            if (slowAuraTimer >= 0.5f)
            {
                SlowEnemies();
                slowAuraTimer = 0f;
            }
        }
        // Timer tick
        poisonAuraTimer += Time.deltaTime;
        slowAuraTimer += Time.deltaTime;
    }

    private void PoisonEnemies()
    {
        foreach (Collider2D enemy in enemiesInAura)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                float poisonDamage = enemy.gameObject.GetComponentInParent<Enemy>().health * (basePoisonDamage * playerStats.PoisonAuraLevel);
                enemy.gameObject.GetComponentInParent<Enemy>().TakeDamage(poisonDamage);
            }
        }
    }

    private void SlowEnemies()
    {
        // Slow enemy movement speed by 5% for each level
        foreach (Collider2D enemy in enemiesInAura)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponentInParent<Enemy>().moveSpeed *= (1 - 0.05f * playerStats.SlowAuraLevel);
                Debug.Log("Enemy slowed");
            }
        }
    }

    private void VoidProjectileAttack()
    {
        // Shoot void projectiles at random enemies in the aura
        enemiesInRange = Physics2D.OverlapCircleAll(transform.position, playerStats.voidRadius * voidProjectilRangeMod, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length <= 0) return;
        int voidProjectileCount = voidProjectileCountBase + playerStats.voidProjectileLevel;
        for (int i = 0; i < voidProjectileCount; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemiesInRange.Length);
            Vector2 direction = (enemiesInRange[randomEnemyIndex].transform.position - transform.position).normalized;
            Quaternion prefabRotation = Quaternion.Euler(0, 0,
                                        Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject projectile = Instantiate(voidProjectilePrefab, transform.position, prefabRotation);
            projectile.GetComponent<Bullet>().Initialize(direction, "Player", voidProjectileSpeed, voidProjectileDamage);
        }
    }

    public IEnumerator VoidProjectileAttackCoroutine()
    {
        while (true)
        {
            VoidProjectileAttack();
            yield return new WaitForSeconds(VPACooldown); // Attack every second
        }
    }

    public void OrbitBallAttack()
    {
        GameObject orbitBall = Instantiate(orbitBallPrefab, transform.position, Quaternion.identity);
        float orbitBallRadius = PlayerStats.instance.voidRadius + orbitBallRadiusBase;
        orbitBall.GetComponent<OrbitBall>().Initialize(transform.position, orbitBallSpeed, orbitBallDamage, orbitBallRadius);
        
        orbitBalls.Add(orbitBall);
    }

}
