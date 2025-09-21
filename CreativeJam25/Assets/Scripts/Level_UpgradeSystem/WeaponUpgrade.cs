using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class WeaponUpgrade : Upgrade
{
    private enum WeaponsUpgradeType
    {
        OrbitBall,
        VoidProjectile,
        Lazer,
        SlowAura,
        PoisonAura
    }

    [SerializeField] private WeaponsUpgradeType upgradeType;
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
            case WeaponsUpgradeType.OrbitBall:
                Debug.Log("Increasing OrbitBall Weapon by one level");
                stats.OrbitBallLevel++;
                if (stats.OrbitBallLevel >= 1)
                {
                    player.GetComponent<Player>().OrbitBallAttack();
                }
                break;
            case WeaponsUpgradeType.VoidProjectile:
                Debug.Log("Increasing AutoCannon Weapon by one level");
                stats.voidProjectileLevel++;
                if (stats.voidProjectileLevel == 1)
                {
                    player.GetComponent<Player>().StartCoroutine(player.GetComponent<Player>().VoidProjectileAttackCoroutine());
                }
                break;
            case WeaponsUpgradeType.Lazer:
                Debug.Log("Increasing Lazer Weapon by one level");
                stats.LazerLevel++;
                if (stats.LazerLevel >= 1)
                {
                    player.GetComponent<Player>().LaserBeamAttack();
                }
                break;
            case WeaponsUpgradeType.SlowAura:
                Debug.Log("Increasing SlowAura Aura Weapon by one level");
                stats.SlowAuraLevel++;
                break;
            case WeaponsUpgradeType.PoisonAura:
                Debug.Log("Increasing PoisonAura Aura Weapon by one level");
                stats.PoisonAuraLevel++;
                break;
            default:
                Debug.LogError("Unknown upgrade type.");
                break;
        }
    }
}