using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int currentWeaponIndex = 0;

    private Vector2 movement;
    private Vector2 lastMovementDirection;
    private bool isWalking;
    private bool isFacingLeft = false;

    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;
    private float attackCooldownTimer;

    private WeaponParent weaponParent;

    public RectTransform meter; // Reference to the meter (UI element)
    public GameObject gameOverUI; // Reference to the GameOver UI

    private const string MeterPositionKey = "MeterPosition";

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();

        // Only enable the assigned weapon
        for (int i = 0; i < weaponSpriteTransforms.Length; i++)
        {
            if (weaponSpriteTransforms[i] != null)
                weaponSpriteTransforms[i].gameObject.SetActive(i == currentWeaponIndex);
        }

        // Load the meter's position from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey(MeterPositionKey))
        {
            float savedX = PlayerPrefs.GetFloat(MeterPositionKey);
            meter.anchoredPosition = new Vector2(savedX, meter.anchoredPosition.y);
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

    // Method to call when an enemy is destroyed
    public void UpdateMeter(int weaponIndex)
    {
        float moveAmount = (weaponIndex == 0) ? 90f : -90f;
        meter.anchoredPosition += new Vector2(moveAmount, 0);

        // Save the new meter position to PlayerPrefs
        PlayerPrefs.SetFloat(MeterPositionKey, meter.anchoredPosition.x);
        PlayerPrefs.Save();

        // Check if GameOver condition is met
        if (meter.anchoredPosition.x <= -450f || meter.anchoredPosition.x >= 450f)
        {
            gameOverUI.SetActive(true);
        }
    }

    public static void ResetMeter()
    {
        PlayerPrefs.DeleteKey(MeterPositionKey);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        // Optional: Clear the saved meter position when quitting the app
        PlayerPrefs.DeleteKey(MeterPositionKey);
        PlayerPrefs.Save();
    }
}
