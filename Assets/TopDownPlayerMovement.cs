using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer; // Main player sprite
    public Animator animator;

    public Transform childSpriteTransform; // The child sprite object
    public SpriteRenderer childSpriteRenderer; // The SpriteRenderer on the child

    private Vector2 movement;
    private bool isWalking;
    private bool isFacingLeft = false;

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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

        // Handle animation
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

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void FlipChild(bool flipLeft)
    {
        // Flip localPosition.x to mirror the child
        Vector3 childPos = childSpriteTransform.localPosition;
        childPos.x = -Mathf.Abs(childPos.x) * (flipLeft ? 1 : -1);
        childSpriteTransform.localPosition = childPos;

        // Flip the child's sprite visually
        childSpriteRenderer.flipX = flipLeft;
    }
}
