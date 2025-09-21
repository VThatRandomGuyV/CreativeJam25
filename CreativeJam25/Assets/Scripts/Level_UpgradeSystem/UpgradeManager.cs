using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//Singleton class to manage XP
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("XP System")]
    [SerializeField] private float xp = 0.0f;

    private float xpToNextLevel = 100.0f;

    [SerializeField] private Slider xpBar;

    [SerializeField] TextMeshProUGUI xpText;

    [SerializeField] TextMeshProUGUI levelText;

    [Header("Score & Time Survived")]
    public int enemiesKilled = 0;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI timeSurvivedText;

    private AudioSource audioSource; //reference to the audio source component
    public UnityEvent OnLevelUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        xpBar.value = xp;
        xpText.text = xp + " / " + xpToNextLevel + " XP";
        levelText.text = "Level "+PlayerStats.instance.level;
        enemiesKilledText.text = enemiesKilled + " killed";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        // Set time survived text
        TimeSpan timeSurvived = Level.Instance.timeSurvived;
        timeSurvivedText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSurvived.Hours, timeSurvived.Minutes, timeSurvived.Seconds);
    }

    public void SetLevelText(int level)
    {
        levelText.text = "Level " + level;
    }

    public void AddXP(float amount)
    {
        xp += amount;
        xpBar.value = xp / xpToNextLevel;
        xpText.text = xp + " / " + xpToNextLevel + " XP";
        if (xp >= xpToNextLevel)
        {
            xp = xp - xpToNextLevel;
            //Make the next level require more xp based on an exponential scale
            xpToNextLevel *= 1.5f;
            PlayerState.instance.currentState = PlayerState.PlayerStates.InMenu; //reset state to normal on level up
            OnLevelUp.Invoke();
            audioSource.PlayOneShot(audioSource.clip);
            xpBar.value = xp / xpToNextLevel;
            xpText.text = xp + " / " + xpToNextLevel + " XP";
            levelText.text = "Level " + PlayerStats.instance.level;
        }
    }
}
