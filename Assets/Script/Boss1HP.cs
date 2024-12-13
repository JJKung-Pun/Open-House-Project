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

    /// <summary>
    /// Apply damage to the boss. Prevents health from dropping below 1.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply.</param>
    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Prevent health from dropping below 1 during regular attacks
        if (currentHealth < 1)
        {
            currentHealth = 1;
        }
        UpdateHealthBar();

        // Trigger Hurt animation
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Hurt");
        }

        // Check if health is exactly 1 to stop actions
        if (currentHealth == 1)
        {
            StopAllActions();
        }
    }

    /// <summary>
    /// Directly sets the boss's health to a specific value. Useful for QTE success.
    /// </summary>
    /// <param name="newHealth">The new health value to set.</param>
    public void SetHealth(int newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            // Trigger death sequence
            StopAllActions();
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Dead");
            }
            StartCoroutine(HandleDeath());
        }
        else if (currentHealth == 1)
        {
            // Stop actions if health reaches 1
            StopAllActions();
        }

        // Optionally, trigger Hurt animation if damage is taken
        if (currentHealth > 0 && newHealth < currentHealth && bossAnimator != null)
        {
            bossAnimator.SetTrigger("Hurt");
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

    /// <summary>
    /// Heal the boss by a specified amount. Clamps health to maxHealth.
    /// </summary>
    /// <param name="healAmount">Amount to heal.</param>
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

        // Optionally, play an idle or warning animation
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Idle"); // Ensure you have an "Idle" trigger in Animator
        }
    }
}
