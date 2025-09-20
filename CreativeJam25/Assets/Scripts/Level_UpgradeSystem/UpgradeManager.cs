using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//Singleton class to manage XP
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [SerializeField] private float xp = 0.0f;

    private float xpToNextLevel = 100.0f;

    [SerializeField] private Slider xpBar;

    [SerializeField] TextMeshProUGUI xpText;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
            OnLevelUp.Invoke();
            xpBar.value = xp / xpToNextLevel;
            xpText.text = xp + " / " + xpToNextLevel + " XP";
        }
    }
}
