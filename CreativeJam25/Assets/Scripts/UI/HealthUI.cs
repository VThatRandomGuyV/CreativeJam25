using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    
    
    [SerializeField] private Slider healthSlider;

    [SerializeField] private TextMeshProUGUI healthFractionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = PlayerStats.instance.maxHealth;
        healthSlider.value = PlayerStats.instance.Health;
        healthSlider.maxValue = PlayerStats.instance.maxHealth;
        healthFractionText.text = PlayerStats.instance.Health + " / " + PlayerStats.instance.maxHealth;
    }

    public void UpdateHealthUI()
    {
        healthSlider.maxValue = (int)PlayerStats.instance.maxHealth;
        healthSlider.value = (int)PlayerStats.instance.Health;
        healthFractionText.text = (int)PlayerStats.instance.Health + " / " + (int)PlayerStats.instance.maxHealth;
    }

    
}
