using Characters;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class LaserBeam : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private bool isActive = false;

    private Collider2D laserCollider;
    private SpriteRenderer spriteRenderer;

    public void Initialize(float damagePerSecond)
    {
        this.damagePerSecond = damagePerSecond;
        isActive = true;

        laserCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateTargetPosition(Vector3 newTargetPosition, Vector3 originPosition)
    {
        this.targetPosition = newTargetPosition;
        // Update laser beam direction and length based on new target position and the player`s position
        Vector3 direction = (targetPosition - originPosition).normalized;
        float distance = Vector3.Distance(originPosition, targetPosition);
        transform.SetPositionAndRotation(originPosition + direction * (distance / 2),
                                            Quaternion.LookRotation(Vector3.forward, direction));
        transform.localScale = new Vector3(1f, distance, 1f); // Assuming the laser sprite is 1 unit tall

        // Update collider and sprite renderer size
        if (laserCollider != null)
        {
            var boxCollider = laserCollider as BoxCollider2D;
            if (boxCollider != null)
            {
                boxCollider.offset = Vector2.zero;
                boxCollider.size = new Vector2(0.1f, distance);
            }
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.size = new Vector2(0.1f, distance);
        }
    }

    private void Update()
    {
        if (!isActive) return;

        // Constantly update the laser position to follow the player
        UpdateTargetPosition(targetPosition, Level.Instance.Player.transform.position);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponentInParent<Enemy>().TakeDamageNoKnockback(damagePerSecond * Time.deltaTime);
        }
    }
}