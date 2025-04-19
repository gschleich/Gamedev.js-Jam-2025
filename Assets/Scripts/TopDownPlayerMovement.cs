using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float attackCooldown = 0.5f;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Animator weaponAnimator;

    public Transform childSpriteTransform;
    public SpriteRenderer childSpriteRenderer;

    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private bool isWalking;
    private bool isFacingLeft = false;

    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;
    private float attackCooldownTimer;

    private WeaponParent weaponParent;

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastMovementDirection = movement.normalized;
        }

        // Mouse position & weapon rotation
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        weaponParent.PointerPosition = mouseWorldPos;

        // Flip player sprite based on pointer position
        if (mouseWorldPos.x < transform.position.x && !isFacingLeft)
        {
            spriteRenderer.flipX = true;
            isFacingLeft = true;
        }
        else if (mouseWorldPos.x > transform.position.x && isFacingLeft)
        {
            spriteRenderer.flipX = false;
            isFacingLeft = false;
        }

        // Attack input
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0f)
        {
            weaponAnimator.SetTrigger("Attack");
            attackCooldownTimer = attackCooldown;
        }

        // Decrease cooldown timers
        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Animation
        if (!isDashing)
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
}
