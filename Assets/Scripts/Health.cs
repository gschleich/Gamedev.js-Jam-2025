using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int currentHealth, maxHealth;
    [SerializeField] private bool isDead = false;

    [Header("UI")]
    [SerializeField] private FloatingHealthbar healthbar;

    [Header("Death Effects")]
    public GameObject deathDustPrefab;

    [Header("Events")]
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    private readonly Color hitColor = new Color(0.811f, 0.341f, 0.239f); // CF573D
    //private readonly Color hitColor = new Color(0.922f, 0.929f, 0.914f); // EBEDE9
    private readonly float hitColorDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine colorChangeCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("Health script requires a SpriteRenderer on the same GameObject.");
        }
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        if (healthbar != null)
        {
            healthbar.UpdateHealthbar(currentHealth, maxHealth);
        }
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead || sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthbar != null)
        {
            healthbar.UpdateHealthbar(currentHealth, maxHealth);
        }

        if (spriteRenderer != null)
        {
            if (colorChangeCoroutine != null)
                StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = StartCoroutine(FlashHitColor());
        }

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;

            if (deathDustPrefab != null)
            {
                Instantiate(deathDustPrefab, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);
        }
    }

    private IEnumerator FlashHitColor()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        spriteRenderer.color = originalColor;
    }
}
