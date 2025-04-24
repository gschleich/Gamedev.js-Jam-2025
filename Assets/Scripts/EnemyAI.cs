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
        if (isFalling)
            return;

        if (knockback != null && knockback.IsBeingKnockedBack())
            return;

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                if (player.position.x > transform.position.x)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;

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
