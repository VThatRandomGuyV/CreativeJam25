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
        //Change gamestart to InGameMenu state
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
            //Attach the upgrade to the panel (Assume the upgrade is a prefab with a script called Upgrade that has a method called ApplyUpgrade)
        }
    }

    public void CloseUpgradeMenu()
    {
        //Which button was pressed?
        //Apply the upgrade to the player.
        //Close the menu and resume the game.
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        //Change gamestate back to Level
    }
}
