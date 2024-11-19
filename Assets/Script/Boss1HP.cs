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

    void Start()
    {
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

        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Hurt");
        }

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
        yield return new WaitForSeconds(2.0f);
        EnemyDefeated.gameObject.SetActive(true);
        enemyHealthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
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

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void StopAllActions()
    {
        bossMovement.StopMovement();
        bossMovement.StopAttacking();
        if (bossAnimator != null)
        {
            bossAnimator.SetBool("IsAttacking", false);
        }
    }
}
