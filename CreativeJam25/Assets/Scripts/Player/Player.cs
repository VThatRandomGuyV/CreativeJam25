using Characters;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{   
    private PlayerStats playerStats;
    private PlayerState playerState;
    private PlayerController playerController;

    private Collider2D[] enemiesInAura;

    private float poisonAuraTimer = 0f;
    private float slowAuraTimer = 0f;
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
        enemiesInAura = Physics2D.OverlapCircleAll(transform.position, playerStats.voidRadius);
        if (playerStats.PoisonAuraLevel >= 0)
        {
            if(poisonAuraTimer >= 0.5f)
            {
                PoisonEnemies();
                poisonAuraTimer = 0f;
            }
        }
        if(playerStats.SlowAuraLevel >= 0)
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
                float poisonDamage = enemy.gameObject.GetComponentInParent<Enemy>().health * (0.02f * playerStats.PoisonAuraLevel);
                enemy.gameObject.GetComponentInParent<Enemy>().TakeDamage(poisonDamage);
            }
        }
    }

    private void SlowEnemies()
    {
        // Slow enemy movement speed by 5% for each level
        foreach(Collider2D enemy in enemiesInAura)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponentInParent<Enemy>().moveSpeed *= (1 - 0.05f * playerStats.SlowAuraLevel);
            }
            Debug.Log("Enemy slowed");
        }
    }
}
