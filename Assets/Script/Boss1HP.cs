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
        bossAnimator = GetComponent<Animator>();
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

            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Hurt");
            }

            if (currentHealth == 1)
            {
                StopAllActions();
                bossQTE.StartQTE(); // Start QTE when health reaches 1
                EnemyDefeated.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateHealthBar()
    {
        if (enemyHealthBar != null)
        {
            enemyHealthBar.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("Dead");
            }
            StartCoroutine(HandleDeath());
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
