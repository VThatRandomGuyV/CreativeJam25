using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float rewardAmount;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradeManager.Instance.AddXP(rewardAmount);
            Destroy(gameObject);
        }
    }
}
