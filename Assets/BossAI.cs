using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHealth = 30;
    private Health health; // Reference to Health script

    [Header("Player Settings")]
    public Transform player;
    public float moveSpeed = 3f;
    public float attackRange = 5f;

    [Header("Teleport and Fire")]
    public GameObject[] teleportPoints;
    public GameObject[] firePoints;
    public GameObject projectilePrefab;
    public float shootRange = 8f;
    public float attackCooldown = 2f;

    [Header("Animator")]
    public Animator bossAnimator;

    private bool isTeleporting = false;
    private bool hasAttacked = false;
    private bool isSecondPhase = false;

    private void Start()
    {
        health = GetComponent<Health>();
        health.InitializeHealth(maxHealth);
        StartCoroutine(BossBehavior());
    }

    private IEnumerator BossBehavior()
    {
        while (true)
        {
            int currentHealth = GetCurrentHealth();

            if (currentHealth > maxHealth / 2)
            {
                if (!isSecondPhase)
                {
                    yield return StartCoroutine(FirstPhase());
                }
            }
            else
            {
                isSecondPhase = true;
                yield return StartCoroutine(SecondPhase());
            }

            yield return null;
        }
    }

    private IEnumerator FirstPhase()
    {
        while (GetCurrentHealth() > maxHealth / 2)
        {
            bossAnimator.Play("Idle");
            yield return new WaitForSeconds(attackCooldown);

            if (!hasAttacked)
            {
                hasAttacked = true;
                bossAnimator.Play("Attack");
                ShootProjectiles(); // move this BEFORE the delay
                yield return new WaitForSeconds(0.5f); // wait for animation
                bossAnimator.Play("Idle");
                yield return new WaitForSeconds(attackCooldown);
            }

            if (!isTeleporting)
            {
                isTeleporting = true;
                bossAnimator.Play("TeleportOut");
                yield return new WaitForSeconds(0.5f);
                TeleportToRandomPoint();
                bossAnimator.Play("TeleportIn");
                yield return new WaitForSeconds(0.5f);
                isTeleporting = false;
            }

            hasAttacked = false;
        }
    }

    private IEnumerator SecondPhase()
    {
        while (GetCurrentHealth() > 0)
        {
            bossAnimator.Play("Idle");
            MoveTowardPlayer();

            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= attackRange && !hasAttacked)
            {
                bossAnimator.Play("Attack");
                ShootProjectiles(); // or you could trigger a melee attack
                hasAttacked = true;
                yield return new WaitForSeconds(attackCooldown);
                hasAttacked = false;
            }

            yield return null;
        }
    }

    private void ShootProjectiles()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= shootRange)
        {
            foreach (var firePoint in firePoints)
            {
                Vector2 direction = (player.position - firePoint.transform.position).normalized;

                GameObject proj = Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity);
                Projectile projectile = proj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.SetDirection(direction);
                }
            }

            // Flip boss to face player
            Vector3 scale = transform.localScale;
            scale.x = player.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void TeleportToRandomPoint()
    {
        int randomIndex = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[randomIndex].transform.position;
    }

    private void MoveTowardPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private int GetCurrentHealth()
    {
        // Using reflection to access private field (since we can't change Health)
        var field = typeof(Health).GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (int)field.GetValue(health);
    }
}
