using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float fireCooldown = 2f;
    public float shootRange = 8f;

    private float fireTimer;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= shootRange)
        {
            // Look at player
            Vector2 direction = (player.position - transform.position).normalized;
            Vector3 scale = transform.localScale;
            scale.x = player.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

            // Shoot if timer elapsed
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Shoot(direction);
                fireTimer = fireCooldown;
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(direction);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}

