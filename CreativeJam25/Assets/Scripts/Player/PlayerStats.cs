using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float health { get; private set; } //how much health the plr has. Feel free to turn it to a float if you want idc;

    public float maxHealth; //max health the plr can have
    public float speed; //how fast the plr moves. Feel free to turn it to a float if you want idc. Idk how fast or slow you want the guy to move

    public float voidRadius;

    SpriteRenderer spriteRenderer; //reference to the sprite renderer component

    [SerializeField] private float invincibilityDuration = 2.0f; //how long the plr is invincible for after taking damage

    private float blinkDuration; //how fast the plr blinks when invincible

    [SerializeField] private float blinkCooldown = 0.5f;

    RaycastHit2D voidAura;
    UnityEvent OnHealthChanged = new UnityEvent(); //event that triggers when health changes

    UnityEvent OnTakeDamage = new UnityEvent(); //event that triggers when the player takes damage

    UnityEvent OnDeath = new UnityEvent(); //event that triggers when the player dies
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        voidAura = Physics2D.CircleCast(transform.position, voidRadius, Vector2.zero, 0);

        //When you press Spacebar take damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10.0f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, voidRadius);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage: " + damage);
        if (PlayerState.instance.currentState == PlayerState.PlayerStates.Dead ||
        PlayerState.instance.currentState == PlayerState.PlayerStates.Damaged)
        {
            Debug.Log("Player is invincible, not taking damage, or dead");
            return;
        }
        health -= damage;
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            health = 0;
            PlayerState.instance.currentState = PlayerState.PlayerStates.Dead;
            Debug.Log("Player died");
            OnDeath.Invoke();
            //trigger death event
        }
        else
        {
            PlayerState.instance.currentState = PlayerState.PlayerStates.Damaged;
            Debug.Log("Player took damage, health is now: " + health);
            StartCoroutine(InvicibilityFrames());
            StartCoroutine(BlinkRenderer());
            OnTakeDamage.Invoke();
            //trigger damaged event
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth *= 1 + (amount / 100);
        health *= 1 + (amount / 100);
        OnHealthChanged.Invoke();
    }

    public void IncreaseSpeed(float amount)
    {
        speed *= 1 + (amount / 100);
    }

    public void IncreaseVoidAura(float amount)
    {
        //increase void aura radius
        voidRadius *= 1 + (amount / 100);
    }
    private IEnumerator InvicibilityFrames()
    {
        blinkDuration = blinkCooldown;
        yield return new WaitForSeconds(invincibilityDuration);
        PlayerState.instance.currentState = PlayerState.PlayerStates.Normal;
    }

    private IEnumerator BlinkRenderer()
    {
        while (PlayerState.instance.currentState == PlayerState.PlayerStates.Damaged)
        {
            if (blinkDuration > 0.0f)
            {
                blinkDuration -= Time.deltaTime;
            }
            else
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                blinkDuration = blinkCooldown;
            }
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.enabled = true;
    }
}
