using System.Collections;
using UnityEngine;

public class HellBossAI : MonoBehaviour
{
    public Transform[] points; // Assign 7 points in Inspector
    public Transform player;   // Reference to the player
    public GameObject enemyPrefab;  // Enemy prefab to spawn
    public Transform[] spawnPoints; // 5 spawn points for enemies in Stage 3

    public float stage1MoveSpeed = 10f;
    public float stage2MoveSpeed = 3.5f;
    public float stage3MoveSpeed = 10f;  // Same as Stage 1

    private int currentPointIndex = 0;
    private bool movingForward = true;

    private Animator animator;
    private Vector3 originalScale;
    private Health health; // Reference to Health script

    private bool hasStartedMoving = false; // NEW: Wait for idle before moving
    private bool hasSpawnedEnemies = false; // NEW: To check if enemies are spawned in Stage 3

    private void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        originalScale = transform.localScale;

        if (animator != null)
        {
            animator.Play("HellBossIdle"); // Start with Idle
        }

        // Wait 5 seconds, then allow movement
        StartCoroutine(IdleBeforeStart());
    }

    private IEnumerator IdleBeforeStart()
    {
        yield return new WaitForSeconds(2.5f);
        hasStartedMoving = true;

        if (animator != null)
        {
            animator.Play("HellBossWalk");
        }
    }

    private void Update()
    {
        if (points.Length == 0 || player == null || health == null) return;

        // Always face the player
        FacePlayer();

        // Don't move until after the 5 second idle
        if (!hasStartedMoving) return;

        int currentHealth = GetCurrentHealth();

        if (currentHealth > 30)
        {
            Stage1Movement();
        }
        else if (currentHealth > 20)
        {
            Stage2Chase();
        }
        else if (currentHealth > 10)
        {
            Stage3MovementAndSpawnEnemies();
        }
        else
        {
            Stage4Chase();
        }

        // Ensure "HellBossWalk" is playing (only during moving)
        if (animator != null && !IsPlaying(animator, "HellBossWalk"))
        {
            animator.Play("HellBossWalk");
        }
    }

    private void Stage1Movement()
    {
        Transform targetPoint = points[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, stage1MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= points.Length)
                {
                    currentPointIndex = points.Length - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    movingForward = true;
                }
            }
        }
    }

    private void Stage2Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, stage2MoveSpeed * Time.deltaTime);
    }

    private void Stage3MovementAndSpawnEnemies()
    {
        // Only spawn enemies once at the start of Stage 3
        if (!hasSpawnedEnemies)
        {
            SpawnEnemies();
            hasSpawnedEnemies = true;
        }

        // Run the boss around the points like in Stage 1
        Transform targetPoint = points[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, stage3MoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentPointIndex++;
                if (currentPointIndex >= points.Length)
                {
                    currentPointIndex = points.Length - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentPointIndex--;
                if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    movingForward = true;
                }
            }
        }
    }

    private void Stage4Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, stage2MoveSpeed * Time.deltaTime);
    }

    private void FacePlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private void SpawnEnemies()
    {
        // Spawn enemies at each of the 5 spawn points
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private bool IsPlaying(Animator anim, string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    private int GetCurrentHealth()
    {
        var field = typeof(Health).GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (int)field.GetValue(health);
    }
}
