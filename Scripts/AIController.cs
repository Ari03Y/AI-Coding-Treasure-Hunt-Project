using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{
    public enum State { Patrolling, Chasing, Attacking, Fleeing }
    public State currentState = State.Patrolling;

    public Transform player;
    public float chaseDistance = 20f;
    public float attackDistance = 15f;
    public float fleeDistance = 30f;
    public float speed = 4f;
    public float rotationSpeed = 2f;

    public GameObject markerProjectile;
    public Transform firePoint;
    public int markerDamage = 3;
    public float fireRate = 0.5f;
    public int markersPerBurst = 10;
    public float burstCooldown = 1.5f;

    public AudioClip shootingSound;

    public List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;
    private Vector3 targetPosition;

    private bool canAttack = true;
    private AudioSource audioSource;

    // Health variables
    public int maxHealth = 50;
    public int currentHealth;

    // Healing variables
    public float healingRate = 2f; // HP per second
    private bool isHealing = false;

    // Health bar UI
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private HealthBar healthBar;
    // private float heightOffset = 10f;

    void Start()
    {
        // Ensure the player reference is set
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if one doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialize patrol
        if (patrolPoints != null && patrolPoints.Count > 0)
        {
            currentState = State.Patrolling;
            currentPatrolIndex = 0;
            targetPosition = patrolPoints[currentPatrolIndex].position;
        }

        // Initialize health
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = State.Chasing;
                }
                else
                {
                    Patrol();
                }
                break;

            case State.Chasing:
                if (distanceToPlayer > chaseDistance)
                {
                    currentState = State.Patrolling;
                }
                else if (distanceToPlayer <= attackDistance)
                {
                    currentState = State.Attacking;
                }
                else
                {
                    ChasePlayer();
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    currentState = State.Chasing;
                }
                else if (canAttack)
                {
                    StartCoroutine(Attack());
                }
                break;

            case State.Fleeing:
                FleeFromPlayer();
                if (distanceToPlayer > fleeDistance && currentHealth >= 10)
                {
                    currentState = State.Patrolling;
                }
                break;
        }

        // Healing when fleeing
        if (currentState == State.Fleeing && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
    }

    void Patrol()
    {
        if (patrolPoints.Count == 0)
            return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;

        // Check if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SelectNextPatrolPoint();
        }
    }

    void SelectNextPatrolPoint()
    {
        if (patrolPoints.Count == 0)
            return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        targetPosition = patrolPoints[currentPatrolIndex].position;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position += direction * speed * Time.deltaTime;
    }

    void FleeFromPlayer()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position += direction * speed * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        canAttack = false;

        for (int i = 0; i < markersPerBurst; i++)
        {
            // Play shooting sound if it's assigned
            if (shootingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootingSound, 0.01f);
            }

            GameObject marker = Instantiate(markerProjectile, firePoint.position, firePoint.rotation);
            Vector3 shootDirection = (player.position - firePoint.position).normalized;
            marker.GetComponent<MarkerProjectile>().Initialize(shootDirection, markerDamage);
            yield return new WaitForSeconds(fireRate);
        }

        yield return new WaitForSeconds(burstCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        // Update health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            // AI dies
            Die();
        }
        else if (currentHealth < 10)
        {
            // Switch to fleeing state
            currentState = State.Fleeing;
        }
    }

    void Die()
    {
        // Destroy health bar
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }

        // Destroy AI GameObject
        Destroy(gameObject);
    }

    IEnumerator HealOverTime()
    {
        isHealing = true;
        while (currentHealth < maxHealth && currentState == State.Fleeing)
        {
            currentHealth += Mathf.RoundToInt(healingRate * Time.deltaTime);
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            // Update health bar
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            yield return null;
        }
        isHealing = false;
    }
}