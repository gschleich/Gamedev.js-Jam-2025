using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
    }

    void Start()
    {
        if (currentHealth == 0) // Only set if not already set
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}