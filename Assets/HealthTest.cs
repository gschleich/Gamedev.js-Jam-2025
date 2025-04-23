using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int currentHealth, maxHealth;

    [SerializeField]
    private bool isDead = false;

<<<<<<< Updated upstream
    [SerializeField] FloatingHealthbar healthbar;

    // private void Awake()
    // {
    //     healthbar = GetComponentInChildren<FloatingHealthbar>();
    // }
=======
    [Header("Death Effects")]
    public GameObject deathDustPrefab;

    [Header("Events")]
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [Header("UI")]
    public HealthBar healthBar;

    private readonly Color hitColor = new Color(0.811f, 0.341f, 0.239f); // CF573D
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
>>>>>>> Stashed changes

    public void InitializeHealth(int healthValue)
    {
        maxHealth = healthValue;
<<<<<<< Updated upstream
        if (healthbar != null)
        {
            healthbar.UpdateHealthbar(currentHealth, maxHealth);
        }
=======
        currentHealth = maxHealth;
>>>>>>> Stashed changes
        isDead = false;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead || sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;
<<<<<<< Updated upstream
        if (healthbar != null)
        {
            healthbar.UpdateHealthbar(currentHealth, maxHealth);
=======
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // just in case

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (spriteRenderer != null)
        {
            if (colorChangeCoroutine != null)
                StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = StartCoroutine(FlashHitColor());
>>>>>>> Stashed changes
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

    private System.Collections.IEnumerator FlashHitColor()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitColorDuration);
        spriteRenderer.color = originalColor;
    }
}
