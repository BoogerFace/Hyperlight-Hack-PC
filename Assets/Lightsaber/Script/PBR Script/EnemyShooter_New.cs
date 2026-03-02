using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter_New : MonoBehaviour
{
    [Header("General")]
    public Transform shootPoint;    // Where raycast starts from
    public Transform gunPoint;      // Where visual trail originates
    public LayerMask layerMask;     // What the raycast hits

    [Header("Shooting")]
    public Vector3 spread = new Vector3(0.06f, 0.06f, 0.06f);
    public TrailRenderer bulletTrail;
    public GameObject projectile;
    public float projectileSpeed = 20f;
    public float fireRate = 1.0f;

    private float lastFireTime = 0f;

    [Header("Health")]
    public int health = 100;

    private Enemy_References enemyReferences;
    private bool isDead = false;

    public AudioSource audioSource;
    public AudioClip shootSound;

    private void Awake()
    {
        enemyReferences = GetComponent<Enemy_References>();

        // 🔹 Auto-find the AudioManager in the scene
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            audioSource = audioManager.GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogWarning("EnemyBossAI could not find an AudioManager with an AudioSource in the scene.");
        }
    }

    private void Update()
    {
        if (isDead) return;
    }

    // --- Shooting logic ---
    public void Shoot()
    {
        if (isDead) return;
        if (Time.time < lastFireTime + fireRate) return;

        lastFireTime = Time.time;
        Vector3 direction = GetDirection();

        if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
        {
            Debug.DrawLine(shootPoint.position, shootPoint.position + direction * 10f, Color.red, 1f);
        }

        if (projectile != null)
        {
            audioSource.PlayOneShot(shootSound);
            GameObject proj = Instantiate(projectile, gunPoint.position, Quaternion.LookRotation(direction));
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = direction * projectileSpeed;
        }

        TrailRenderer trail = Instantiate(bulletTrail, gunPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, hit));
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-spread.x, spread.x),
            Random.Range(-spread.y, spread.y),
            Random.Range(-spread.z, spread.z)
        );
        return direction.normalized;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }

    // --- Damage and death ---
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Disable shooting and movement
        this.enabled = false;
        if (enemyReferences != null)
            enemyReferences.enabled = false;

        // Play death animation
        if (TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetTrigger("Die");
        }

        // Disable colliders so player can pass through
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Optional: destroy after 5 seconds
        Destroy(gameObject, 5f);
    }
}
