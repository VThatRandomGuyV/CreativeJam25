using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Scriptable Objects/StatUpgrade")]

public class StatUpgrade : Upgrade
{
    private enum StatUpgradeType
    {
        Health,
        Speed,
        VoidAura
    }

    [SerializeField] private float upgradeAmount = 0;

    [SerializeField] private StatUpgradeType upgradeType;

    public float UpgradeAmount { get { return upgradeAmount; } set { upgradeAmount = value; } }

    public override void ApplyUpgrade(GameObject player)
    {

        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("PlayerStats component not found on player.");
            return;
        }

        switch (upgradeType)
        {
            case StatUpgradeType.Health:
                Debug.Log("Applying Health Upgrade by ");
                stats.IncreaseMaxHealth(upgradeAmount);
                break;
            case StatUpgradeType.Speed:
                Debug.Log("Applying Speed Upgrade by ");
                stats.IncreaseSpeed(upgradeAmount);
                break;
            case StatUpgradeType.VoidAura:
                Debug.Log("Applying Void Aura Upgrade by ");
                stats.IncreaseVoidAura(upgradeAmount);
                break;
            default:
                Debug.LogError("Unknown upgrade type.");
                break;
        }
    }
}
