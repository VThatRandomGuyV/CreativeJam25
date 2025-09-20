using TMPro;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;

    public AudioSource audioSource{ get; private set; } //reference to the audio source component
    Upgrade upgrade;

    Upgrade panelUpgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (description == null)
        {
            Debug.LogError("Description not found in UpgradePanel");
        }
        //Setup a button listener for the button in this panel
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetUpgrade(Upgrade upgrade)
    {
        panelUpgrade = upgrade;
        title.text = upgrade.Name;
        description.text = upgrade.Description;
    }

    public void OnClick()
    {
        Debug.Log("Upgrade " + panelUpgrade.Name + " selected!");
        panelUpgrade.ApplyUpgrade(player: GameObject.FindWithTag("Player"));
    }
}
