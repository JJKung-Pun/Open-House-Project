using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; 
    public int currentHealth;
    public CanvasGroup EnemyDefeated;
    public Slider enemyHealthBar;

    void Start()
    {
        EnemyDefeated.gameObject.SetActive(false);
        currentHealth = maxHealth;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = maxHealth;
            enemyHealthBar.value = currentHealth;
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }

        if (currentHealth == 0)
        {
            EnemyDefeated.gameObject.SetActive(true);
            enemyHealthBar.gameObject.SetActive(false); 
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }
    }
}
