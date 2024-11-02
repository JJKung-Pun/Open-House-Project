using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss1HP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public CanvasGroup EnemyDefeated;
    public Slider enemyHealthBar;
    private Boss1QTE bossQTE;
    private Boss1Movement bossMovement;

    void Start()
    {
        EnemyDefeated.gameObject.SetActive(false);
        currentHealth = maxHealth;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = maxHealth;
            enemyHealthBar.value = currentHealth;
        }
        bossQTE = GetComponent<Boss1QTE>();
        bossMovement = GetComponent<Boss1Movement>();
    }

    public void Damage(int damageAmount)
    {
        if (bossQTE != null && !bossQTE.IsInQTE())
        {
            currentHealth -= damageAmount;
            if (currentHealth < 1)
            {
                currentHealth = 1;
            }
            UpdateHealthBar();
            if (currentHealth == 1)
            {
                bossQTE.StartQTE();
                bossMovement.StopMovement();
                EnemyDefeated.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            EnemyDefeated.gameObject.SetActive(true);
            enemyHealthBar.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }
}
