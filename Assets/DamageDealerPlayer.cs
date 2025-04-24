using UnityEngine;

public class DamageDealerPlayer : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return; // Ignore player
        }

        Health targetHealth = collision.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.GetHit(damageAmount, gameObject);
        }
    }
}
