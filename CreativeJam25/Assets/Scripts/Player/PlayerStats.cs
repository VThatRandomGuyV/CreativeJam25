using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float health { get; private set; } //how much health the plr has. Feel free to turn it to a float if you want idc;

    public float maxHealth; //max health the plr can have
    public float speed; //how fast the plr moves. Feel free to turn it to a float if you want idc. Idk how fast or slow you want the guy to move

    UnityEvent OnHealthChanged = new UnityEvent(); //event that triggers when health changes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        OnHealthChanged.Invoke();
        if (health <= 0)
        {
            PlayerState.instance.currentState = PlayerState.PlayerStates.Dead;
            //trigger death event
        }
        else
        {
            PlayerState.instance.currentState = PlayerState.PlayerStates.Damaged;
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
    }
}
