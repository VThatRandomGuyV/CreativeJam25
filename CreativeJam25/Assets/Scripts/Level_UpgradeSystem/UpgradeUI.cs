using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    public GameObject[] upgradePanels = new GameObject[3];

    [SerializeField] private Upgrade[] upgrades; // Array to hold the upgrades

    public List<Upgrade> unSelectedUpgrades = new List<Upgrade>();

    AudioSource audioSource; //reference to the audio source component

    [SerializeField] AudioClip buttonSound;

    public UnityEngine.Events.UnityEvent OnUpgradeSelected = new();

    public void Start()
    {
        UpgradeManager.Instance.OnLevelUp.AddListener(OpenUpgradeMenu);
        for (int i = 0; i < upgradePanels.Length; i++)
        {
            upgradePanels[i].GetComponent<UpgradePanel>().GetComponent<Button>().onClick.AddListener(CloseUpgradeMenu);
        }
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenUpgradeMenu()
    {
        gameObject.SetActive(true);
        //Reset the unSelected upgrades
        for (int i = 0; i < upgrades.Length; i++)
        {
            unSelectedUpgrades.Add(upgrades[i]);
        }
        //Change gamestart to InGameMenu state
        Time.timeScale = 0f; // Pause the game
        UpgradeRandomizer();
    }

    private void UpgradeRandomizer()
    {
        //Load all upgrades from Resoures/Upgrades
        //There are 2 types of upgrades: Weapon and Ability
        //Randomly select upgrades equal to the size of upgradePanels from the list of all upgrades
        for (int i = 0; i < upgradePanels.Length; i++)
        {
            int upgradeIndex = Random.Range(0, unSelectedUpgrades.Count);
            Upgrade randomUpgrade = unSelectedUpgrades[upgradeIndex];
            //Attach the upgrade to the panel (Assume the upgrade is a prefab with a script called Upgrade that has a method called ApplyUpgrade).
            //Change the Description to fit the upgrade description
            //If it is a stat upgrade, have description say "Increases [stat] by [amount]%"
            if (randomUpgrade is StatUpgrade statUpgrade)
            {
                statUpgrade.UpgradeAmount = Random.Range(1, 100); // Random amount between 5% and 20%
                randomUpgrade.Description = "Increases " + statUpgrade.upgradeType + " by " + statUpgrade.UpgradeAmount + "%";
            }
            else if (randomUpgrade is WeaponUpgrade weaponUpgrade)
            {
                randomUpgrade.Description = "Increases " + weaponUpgrade.GetType().Name + "'s level by 1";
            }
            upgradePanels[i].GetComponent<UpgradePanel>().SetUpgrade(randomUpgrade);
            unSelectedUpgrades.RemoveAt(upgradeIndex);
        }
    }

    public void CloseUpgradeMenu()
    {
        StartCoroutine(PlayButtonSoundAndClose());
        OnUpgradeSelected.Invoke();
    }

    public IEnumerator PlayButtonSoundAndClose()
    {
        unSelectedUpgrades.Clear();
        audioSource.PlayOneShot(audioSource.clip = buttonSound);
        yield return new WaitForSecondsRealtime(0.37f);
        PlayerState.instance.currentState = PlayerState.PlayerStates.Normal; //reset state to normal on closing menu
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        //Change gamestate back to Level
    }
}
