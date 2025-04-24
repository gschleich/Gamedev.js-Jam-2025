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

    public Animator[] weaponAnimators = new Animator[4];
    public Transform[] weaponSpriteTransforms = new Transform[4];
    public SpriteRenderer[] weaponSpriteRenderers = new SpriteRenderer[4];

    private int currentWeaponIndex = 0;

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

        // Only enable the assigned weapon
        for (int i = 0; i < weaponSpriteTransforms.Length; i++)
        {
            if (weaponSpriteTransforms[i] != null)
                weaponSpriteTransforms[i].gameObject.SetActive(i == currentWeaponIndex);
        }
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
        if (weaponParent != null)
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

        // Weapon switching
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeapon(3);

        // Hold-to-attack input
        if (Input.GetMouseButton(0) && attackCooldownTimer <= 0f)
        {
            if (weaponAnimators[currentWeaponIndex] != null)
            {
                weaponAnimators[currentWeaponIndex].SetTrigger("Attack");
                attackCooldownTimer = attackCooldown;
            }
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
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f)
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

    private void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weaponAnimators.Length || index == currentWeaponIndex)
            return;

        // Disable current weapon if valid
        if (weaponSpriteTransforms[currentWeaponIndex] != null)
            weaponSpriteTransforms[currentWeaponIndex].gameObject.SetActive(false);

        // Enable new weapon if valid
        if (weaponSpriteTransforms[index] != null)
            weaponSpriteTransforms[index].gameObject.SetActive(true);

        currentWeaponIndex = index;
    }
}
