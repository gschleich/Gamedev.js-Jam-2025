using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;

    void Start()
    {
        // Get the mouse position in world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Calculate direction from this position to the mouse position
        moveDirection = (mouseWorldPos - transform.position).normalized;

        // Set rotation to face the direction
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy the projectile after 3 seconds
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            // Optionally add logic here for damaging the enemy
        }
    }
}
