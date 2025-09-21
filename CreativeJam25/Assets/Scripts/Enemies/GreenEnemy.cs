using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Collider2D))]
    public class GreenEnemy : Enemy
    {
        [Header(nameof(GreenEnemy))]
        [SerializeField] private Collider2D punchFistCollider;
        [SerializeField] private Collider2D punchFistColliderTrigger;

        private Collider2D buffTriggerCollider;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = Level.Instance.Player;
            buffTriggerCollider = GetComponent<Collider2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("Green Shield Target: " + collision.gameObject.tag);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponentInParent<Enemy>().ShieldYourself(50f);
            }   
        }
    }
}
