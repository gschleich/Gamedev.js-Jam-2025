// Experimental
using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float separationRadius = 1.5f;
    public float separationStrength = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GarrettsKnockback knockback;

    private bool isFalling = false;

    public void EnableGravityFall()
    {
        isFalling = true;
        rb.gravityScale = 1f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<GarrettsKnockback>();
    }

    void FixedUpdate()
    {
        if (isFalling || (knockback != null && knockback.IsBeingKnockedBack()))
            return;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                Vector2 separation = CalculateSeparation();

                Vector2 finalDirection = (directionToPlayer + separation * separationStrength).normalized;

                rb.MovePosition(rb.position + finalDirection * moveSpeed * Time.fixedDeltaTime);

                spriteRenderer.flipX = player.position.x < transform.position.x;

                if (!IsPlaying("Walk"))
                    animator.Play("Walk");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;

                if (!IsPlaying("Idle"))
                    animator.Play("Idle");
            }
        }
    }

    Vector2 CalculateSeparation()
    {
        Vector2 separation = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != this.gameObject && collider.CompareTag("Enemy"))
            {
                Vector2 awayFromOther = transform.position - collider.transform.position;
                if (awayFromOther != Vector2.zero)
                {
                    separation += awayFromOther.normalized / awayFromOther.magnitude;
                }
            }
        }

        return separation;
    }

    bool IsPlaying(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}

// OLD
// using UnityEngine;

// public class EnemyAI : MonoBehaviour
// {
//     public Transform player;
//     public float moveSpeed = 3f;
//     public float chaseRange = 5f;

//     private Rigidbody2D rb;
//     private Animator animator;
//     private SpriteRenderer spriteRenderer;
//     private GarrettsKnockback knockback;

//     private bool isFalling = false;

//     public void EnableGravityFall()
//     {
//         isFalling = true;
//         rb.gravityScale = 1f;
//     }

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         knockback = GetComponent<GarrettsKnockback>();
//     }

//     void FixedUpdate()
//     {
//         if (isFalling)
//             return;

//         if (knockback != null && knockback.IsBeingKnockedBack())
//             return;

//         if (player != null)
//         {
//             float distanceToPlayer = Vector2.Distance(transform.position, player.position);

//             if (distanceToPlayer <= chaseRange)
//             {
//                 Vector2 direction = (player.position - transform.position).normalized;
//                 rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

//                 if (player.position.x > transform.position.x)
//                     spriteRenderer.flipX = false;
//                 else
//                     spriteRenderer.flipX = true;

//                 if (!IsPlaying("Walk"))
//                     animator.Play("Walk");
//             }
//             else
//             {
//                 rb.linearVelocity = Vector2.zero;

//                 if (!IsPlaying("Idle"))
//                     animator.Play("Idle");
//             }
//         }
//     }

//     bool IsPlaying(string animationName)
//     {
//         return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
//     }

//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, chaseRange);
//     }
// }
