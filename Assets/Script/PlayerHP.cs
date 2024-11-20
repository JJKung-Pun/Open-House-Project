using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public GameObject youDiedPanel;
    private Boss1Attack bossAttack;
    private Animator animator;  // Add a reference to the Animator component

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        youDiedPanel.SetActive(false);
        bossAttack = FindObjectOfType<Boss1Attack>();

        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        // Trigger take damage animation
        if (animator != null)
        {
            animator.SetTrigger("takeDamage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Trigger die animation
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }

        youDiedPanel.SetActive(true);
        gameObject.SetActive(false);

        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            Destroy(playerController);
        }

        Boss1Attack bossAttack = FindObjectOfType<Boss1Attack>();
        if (bossAttack != null)
        {
            bossAttack.StopTargetingPlayer();
        }

        PlayerAttackAnimation playerAttackAnimation = GetComponent<PlayerAttackAnimation>();
        if (playerAttackAnimation != null)
        {
            Destroy(playerAttackAnimation);
        }
    }
}
