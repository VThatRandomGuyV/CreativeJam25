using UnityEngine;

using Characters;

public class OrbitBall : MonoBehaviour
{
    private float speed;
    private float damage;
    private float orbitRadius;
    private Vector3 centerPosition;

    private float angle = 0.0f;

    public void Initialize(Vector3 initialCenterPosition, float speed = 5f, float damage = 5f, float orbitRadius = 1.5f)
    {
        this.speed = speed;
        this.damage = damage;
        this.orbitRadius = orbitRadius;
        centerPosition = initialCenterPosition;
        angle = 0f; // Start angle
    }

    void Update()
    {
        // update center position in case the player moves
        centerPosition = Level.Instance.Player.transform.position;

        angle += speed * Time.deltaTime; // Increment angle based on speed
        float x = centerPosition.x + Mathf.Cos(angle) * orbitRadius;
        float y = centerPosition.y + Mathf.Sin(angle) * orbitRadius;
        transform.position = new Vector3(x, y, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
        }
    }
}