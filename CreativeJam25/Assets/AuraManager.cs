using UnityEngine;

public class AuraManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer auraSpriteRenderer;
    [SerializeField] private ParticleSystem auraParticleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the aura sprite renderer
        auraSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        auraParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the aura sprite size based on the player's void radius
        if (Level.Instance != null && Level.Instance.Player != null)
        {
            float voidRadius = Level.Instance.Player.GetComponent<PlayerStats>().voidRadius + 1.0f; // Add some padding
            auraSpriteRenderer.transform.localScale = new Vector3(voidRadius, voidRadius, voidRadius);

            // Update particle system shape x position
            var shape = auraParticleSystem.shape;
            shape.position = new Vector3(voidRadius/4, 1f, 1f);
        }
    }
}
