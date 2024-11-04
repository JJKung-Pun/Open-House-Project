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
    private Animator bossAnimator;

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
        bossAnimator = GetComponent<Animator>(); // Initialize Animator
    }

    public void Damage(int damageAmount)
    {
        if (bossQTE != null && !bossQTE.IsInQTE())
        {
            currentHealth -= damageAmount;

            // Trigger hurt animation
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Hurt");
            }

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
            Debug.Log("Current health is zero or below. Triggering Dead animation.");
            // Trigger death animation
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Dead");
            }

            // Additional logic to handle death after the animation plays
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Wait for the death animation to complete (adjust duration as needed)
        yield return new WaitForSeconds(2.0f); 

        EnemyDefeated.gameObject.SetActive(true);
        enemyHealthBar.gameObject.SetActive(false);
        gameObject.SetActive(false); // Deactivate the boss after death
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
