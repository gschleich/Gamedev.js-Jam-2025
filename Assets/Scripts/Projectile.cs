using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 moveDirection;

    void Start()
    {
        Destroy(gameObject, 3f); // 💣 Destroy after 3 seconds
    }

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}


