using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Characters
{
    public class GreenEnemy : Enemy
    {
        [Header(nameof(GreenEnemy))]
        [SerializeField] private Collider2D buffCollider;

        private readonly List<Enemy> _shieldedEnemies = new();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            buffCollider = GetComponent<Collider2D>();
            if (!buffCollider)
            {
                Debug.Log("buff collider is null");
            }

            Initialize(Level.Instance.Player, null);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void BuffEnemies()
        {
            //OnTriggerEnter2D(buffCollider);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Green Shield Target: " + collision.gameObject.tag);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().ShieldYourself(collision.gameObject.GetComponentInParent<Enemy>().health);
            }   
        }
    }
}
