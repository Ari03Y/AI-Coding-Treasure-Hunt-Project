using UnityEngine;

public class PaperProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set the velocity of the projectile
        rb.velocity = -transform.right * speed;

        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with the player
        if (other.CompareTag("Player"))
            return;

        // Check if the projectile hit an enemy
        AIController enemy = other.GetComponent<AIController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            // Destroy the projectile when it hits any other object
            Destroy(gameObject);
        }
    }
}