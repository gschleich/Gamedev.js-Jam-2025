using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GarrettsKnockback : MonoBehaviour
{
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private EnemyAI enemyAI;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        GetComponent<Health>().OnHitWithReference.AddListener(ApplyKnockback);
    }

    void ApplyKnockback(GameObject sender)
    {
        if (sender == null) return;

        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public bool IsBeingKnockedBack()
    {
        return isKnockedBack;
    }
}