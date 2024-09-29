using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100; 
    public int currentHealth;
    public CanvasGroup Die;
    public Slider healthBar;

    void Start()
    {
        Die.gameObject.SetActive(false);
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth == 0)
        {
            Die.gameObject.SetActive(true);
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.value = currentHealth;

        Debug.Log("Player took " + amount + " damage. Current health: " + currentHealth);

        if (currentHealth == 0)
        {
            Die.gameObject.SetActive(true);
        }
    }
}
