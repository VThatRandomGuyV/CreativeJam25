using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Scriptable Objects/StatUpgrade")]

public class StatUpgrade : Upgrade
{
    private int SpeedUpgradeCount = 0;
    private int VoidAuraUpgradeCount = 0;

    public enum StatUpgradeType
    {
        Health,
        Speed,
        VoidAura
    }

    [SerializeField] private float upgradeAmount = 0;

    [SerializeField] public StatUpgradeType upgradeType;

    public float UpgradeAmount { get { return upgradeAmount; } set { upgradeAmount = value; } }

    public void Start()
    {
        upgradeName = upgradeType.ToString() + " Upgrade";
    }

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
                stats.IncreaseMaxHealth(upgradeAmount);
                break;
            case StatUpgradeType.Speed:
                stats.IncreaseSpeed(upgradeAmount - SpeedUpgradeCount * 2);
                SpeedUpgradeCount++;
                break;
            case StatUpgradeType.VoidAura:
                stats.IncreaseVoidAura(upgradeAmount - VoidAuraUpgradeCount * 2);
                VoidAuraUpgradeCount++;
                break;
            default:
                Debug.LogError("Unknown upgrade type.");
                break;
        }
    }
}
