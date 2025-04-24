using UnityEngine;

public class EnemyGravityController : MonoBehaviour
{
    private bool playerDestroyed = false;

    void Update()
    {
        // Check if player exists in the scene
        if (!playerDestroyed && GameObject.FindGameObjectWithTag("Player") == null)
        {
            playerDestroyed = true;
            SetEnemyGravity();
        }
    }

    void SetEnemyGravity()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
            }

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.EnableGravityFall();
            }
        }
    }
}