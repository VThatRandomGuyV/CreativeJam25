using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance;
    public enum PlayerStates
    {
        Normal,
        Damaged,
        Dead,
        InMenu,
        Win
    }

    public PlayerStates currentState;

    public void Awake()
    {
        instance = this;
        currentState = PlayerStates.Normal;
    }

    void Start()
    {
    }
}
