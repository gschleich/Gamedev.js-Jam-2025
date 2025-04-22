using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float chaseRange = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GarrettsKnockback knockback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<GarrettsKnockback>();
    }

    void FixedUpdate()
    {
        // Prevent AI movement while being knocked back
        if (knockback != null && knockback.IsBeingKnockedBack())
            return;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                // Chase the player
                Vector2 direction = (player.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                // Flip to face player
                if (player.position.x > transform.position.x)
                    spriteRenderer.flipX = false; // face right
                else
                    spriteRenderer.flipX = true;  // face left

                // Play walk animation if not already playing
                if (!IsPlaying("Walk"))
                    animator.Play("Walk");
            }
            else
            {
                // Go idle
                rb.linearVelocity = Vector2.zero;

                // Play idle animation if not already playing
                if (!IsPlaying("Idle"))
                    animator.Play("Idle");
            }
        }
    }

    bool IsPlaying(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
