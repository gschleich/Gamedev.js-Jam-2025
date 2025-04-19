using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public Transform childSpriteTransform;
    public SpriteRenderer childSpriteRenderer;

    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private bool isWalking;
    private bool isFacingLeft = false;

    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastMovementDirection = movement.normalized;
        }

        // Flip logic
        if (movement.x < 0 && !isFacingLeft)
        {
            spriteRenderer.flipX = true;
            FlipChild(true);
            isFacingLeft = true;
        }
        else if (movement.x > 0 && isFacingLeft)
        {
            spriteRenderer.flipX = false;
            FlipChild(false);
            isFacingLeft = false;
        }

        // Animation
        if (!isDashing) // Don't override dash animation
        {
            bool currentlyWalking = movement != Vector2.zero;

            if (currentlyWalking && !isWalking)
            {
                animator.Play("Walk");
                isWalking = true;
            }
            else if (!currentlyWalking && isWalking)
            {
                animator.Play("Idle");
                isWalking = false;
            }
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;

            animator.Play("Dash");
        }

        // Update dash cooldown
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + lastMovementDirection * dashSpeed * Time.fixedDeltaTime);
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0f)
            {
                isDashing = false;

                // Transition out of Dash animation
                if (movement != Vector2.zero)
                {
                    animator.Play("Walk");
                    isWalking = true;
                }
                else
                {
                    animator.Play("Idle");
                    isWalking = false;
                }
            }
        }
        else
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void FlipChild(bool flipLeft)
    {
        Vector3 childPos = childSpriteTransform.localPosition;
        childPos.x = -Mathf.Abs(childPos.x) * (flipLeft ? 1 : -1);
        childSpriteTransform.localPosition = childPos;

        childSpriteRenderer.flipX = flipLeft;
    }
}
