using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            return; // Ignore enemies
        }

        Health targetHealth = collision.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.GetHit(damageAmount, gameObject);
        }
    }
}
