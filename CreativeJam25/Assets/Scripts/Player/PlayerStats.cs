using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerStats : MonoBehaviour
{
    public float Health { get; private set; } //how much health the plr has. Feel free to turn it to a float if you want idc;

    public float maxHealth; //max health the plr can have
    public float speed; //how fast the plr moves. Feel free to turn it to a float if you want idc. Idk how fast or slow you want the guy to move
    public int level = 1;

    public float voidRadius;

    SpriteRenderer spriteRenderer; //reference to the sprite renderer component

    [SerializeField] private float invincibilityDuration = 2.0f; //how long the plr is invincible for after taking damage

    [SerializeField] private float blinkCooldown = 0.5f;

    AudioSource audioSource; //reference to the audio source component

    [SerializeField] AudioClip damageSound; //sound that plays when the plr takes damage

    [SerializeField] AudioClip deathSound; //sound that plays when the plr dies

    RaycastHit2D voidAura;
    UnityEvent OnHealthChanged = new(); //event that triggers when health changes

    UnityEvent OnTakeDamage = new(); //event that triggers when the player takes damage

    UnityEvent OnDeath = new(); //event that triggers when the player dies

    private float blinkDuration; //how fast the plr blinks when invincible

    void Start()
    {
        Health = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        voidAura = Physics2D.CircleCast(transform.position, voidRadius, Vector2.zero, 0);

        //When you press Spacebar take damage
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10.0f);
        }

        // check if player is flipped

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, voidRadius);
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            // Debug: Set enemy`s damage to negative to ignore damage
            return;
        }

        if (PlayerState.instance.currentState == PlayerState.PlayerStates.Dead ||
        PlayerState.instance.currentState == PlayerState.PlayerStates.Damaged)
        {
            return;
        }
        Health -= damage;
        OnHealthChanged.Invoke();
        if (Health <= 0)
        {
            Health = 0;
            PlayerState.instance.currentState = PlayerState.PlayerStates.Dead;
            Debug.Log("Player died");
            OnDeath.Invoke();
            audioSource.PlayOneShot(deathSound);
            //trigger death event
        }
        else
        {
            PlayerState.instance.currentState = PlayerState.PlayerStates.Damaged;
            audioSource.PlayOneShot(damageSound);
            StartCoroutine(InvicibilityFrames());
            StartCoroutine(BlinkRenderer());
            OnTakeDamage.Invoke();
            //trigger damaged event
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth *= 1 + (amount / 100);
        Health *= 1 + (amount / 100);
        OnHealthChanged.Invoke();
    }

    public void LevelUp()
    {
        level += 1;

        if (level % 4 == 0)
        {
            GetComponentInChildren<Animator>().SetInteger("level", level/4);
        }
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
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkDuration);
        }
        spriteRenderer.enabled = true;
    }
}
