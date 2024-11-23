// using UnityEngine;

// public class HomingProjectile : MonoBehaviour
// {
//     public Transform target;        // Target to follow
//     public float speed = 5f;        // Speed of the projectile
//     public int damage = 3;          // Damage dealt by the projectile
//     public float rotateSpeed = 200f; // Rotation speed for homing

//     public void Initialize(Transform enemyTarget, int projectileDamage)
//     {
//         target = enemyTarget;
//         damage = projectileDamage;
//     }

//     void Update()
//     {
//         if (target == null)
//         {
//             Destroy(gameObject); // Destroy the projectile if the target no longer exists
//             return;
//         }

//         // Move towards the target
//         Vector3 direction = (target.position - transform.position).normalized;
//         Quaternion lookRotation = Quaternion.LookRotation(direction);
//         transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

//         transform.Translate(Vector3.forward * speed * Time.deltaTime);
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.transform == target)
//         {
//             // Damage the target if it has a health component
//             AIHealth enemyHealth = other.GetComponent<AIHealth>();
//             if (enemyHealth != null)
//             {
//                 enemyHealth.TakeDamage(damage);
//             }

//             // Destroy the projectile after impact
//             Destroy(gameObject);
//         }
//     }
// }
