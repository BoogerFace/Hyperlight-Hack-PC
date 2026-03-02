using UnityEngine;

public class BombTrigger : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionVFX;
    public float explosionDuration = 2f;
    public float damage = 50f;
    public float radius = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Melee"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionDuration);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Melee"))
            {
                // Melee enemy
                if (hit.TryGetComponent(out EnemyMeleeAI melee))
                    melee.TakeDamage((int)damage);

                // Shooter enemy
                if (hit.TryGetComponent(out EnemyShooter_New shooter))
                    shooter.TakeDamage((int)damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
