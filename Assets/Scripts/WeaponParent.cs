using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    [Header("Attack Settings")]
    public Transform[] circleOrigins = new Transform[4];
    public int currentOriginIndex = 0;
    public float radius;

    private void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (circleOrigins == null || circleOrigins.Length == 0) return;

        Gizmos.color = Color.blue;
        Transform origin = GetCurrentOrigin();
        if (origin != null)
        {
            Gizmos.DrawWireSphere(origin.position, radius);
        }
    }

    public void DetectColliders()
    {
        Transform origin = GetCurrentOrigin();
        if (origin == null) return;

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(origin.position, radius))
        {
            if (collider.TryGetComponent<Health>(out var health))
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }

    private Transform GetCurrentOrigin()
    {
        if (circleOrigins == null || circleOrigins.Length <= currentOriginIndex || currentOriginIndex < 0)
            return null;
        return circleOrigins[currentOriginIndex];
    }
}
