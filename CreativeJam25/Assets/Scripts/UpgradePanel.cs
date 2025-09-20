using TMPro;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    TextMeshProUGUI title;
    TextMeshProUGUI description;

    Upgrade upgrade;

    Upgrade panelUpgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        //Setup a button listener for the button in this panel
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
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
        //panelUpgrade.ApplyUpgrade(Player.Instance.gameObject);
    }
}
