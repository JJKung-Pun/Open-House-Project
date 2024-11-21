using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss1HP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public CanvasGroup EnemyDefeated;
    public Slider enemyHealthBar;
    private Boss1Movement bossMovement;
    private Animator bossAnimator;
    public CanvasGroup MainMenu;

    void Start()
    {
        // Initialize components and variables
        EnemyDefeated.gameObject.SetActive(false);
        currentHealth = maxHealth;

        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = maxHealth;
            enemyHealthBar.value = currentHealth;
        }

        bossMovement = GetComponent<Boss1Movement>();
        bossAnimator = GetComponent<Animator>();
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();

        // Trigger Hurt animation
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Hurt");
        }

        // Handle death if health is 0 or below
        if (currentHealth <= 0)
        {
            StopAllActions();
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Dead");
            }
            StartCoroutine(HandleDeath());
        }
    }

    public void UpdateHealthBar()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }
    }

    private IEnumerator HandleDeath()
    {
        // Wait before showing the UI
        yield return new WaitForSeconds(2.0f);

        // Show the enemy defeated UI
        EnemyDefeated.gameObject.SetActive(true);

        // Hide the health bar
        if (enemyHealthBar != null)
        {
            enemyHealthBar.gameObject.SetActive(false);
        }

        // Show the Main Menu
        if (MainMenu != null)
        {
            MainMenu.alpha = 1;
            MainMenu.interactable = true;
            MainMenu.blocksRaycasts = true;
        }

        // Deactivate the boss GameObject
        gameObject.SetActive(false);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        // Clamp health to the max value
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void StopAllActions()
    {
        // Stop movement and attacking actions
        if (bossMovement != null)
        {
            bossMovement.StopMovement();
            bossMovement.StopAttacking();
        }

        // Stop attack animations
        if (bossAnimator != null)
        {
            bossAnimator.SetBool("IsAttacking", false);
        }
    }
}
