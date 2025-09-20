using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance;
    public enum PlayerStates
    {
        Normal,
        Damaged,
        Dead
    }

    public PlayerStates currentState;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentState = PlayerStates.Normal;
    }

    void Start()
    {
    }


}
