using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject[] upgradePanels = new GameObject[3];

    public void Start()
    {
        UpgradeManager.Instance.OnLevelUp.AddListener(OpenUpgradeMenu);
        gameObject.SetActive(false);
    }

    public void OpenUpgradeMenu()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game
                             // Switch to the UI input map
                             //Randomly select upgrades for panels.
        UpgradeRandomizer();
    }

    private void UpgradeRandomizer()
    {
        //Load all upgrades from Resoures/Upgrades
        //There are 2 types of upgrades: Weapon and Ability
        //GameObject[] allUpgrades = Resources.LoadAll<GameObject>("Upgrades");
        //Randomly select upgrades equal to the size of upgradePanels from the list of all upgrades
        for (int i = 0; i < upgradePanels.Length; i++)
        {
            TextMeshProUGUI title = upgradePanels[i].transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI description = upgradePanels[i].transform.Find("Description").GetComponent<TextMeshProUGUI>();
            title.text = "Upgrade " + (i + 1);
            description.text = "This is a description of upgrade " + (i + 1);
        }
    }

    public void CloseUpgradeMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
                             // Switch back to the gameplay input map
    }
}
