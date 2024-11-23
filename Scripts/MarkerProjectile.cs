using UnityEngine;

public class MarkerProjectile : MonoBehaviour
{
    public float speed = 15f;
    private int damage;
    private Vector3 direction;

    public void Initialize(Vector3 shootDirection, int damageAmount)
    {
        direction = shootDirection.normalized;
        damage = damageAmount;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
