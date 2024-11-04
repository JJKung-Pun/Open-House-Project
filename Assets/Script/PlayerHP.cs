using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public GameObject youDiedPanel;
    public GameObject playerSprite; // Reference to the player sprite
    private Boss1Attack bossAttack;
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        youDiedPanel.SetActive(false);
        bossAttack = FindObjectOfType<Boss1Attack>();
        animator = playerSprite.GetComponent<Animator>(); // Initialize Animator
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (animator != null)
        {
            animator.SetTrigger("takeDamage"); // Play damage animation
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("isDead"); // Play death animation
        }

        youDiedPanel.SetActive(true);

        PlayerController playerController = GetComponent<PlayerController>();
    if (playerController != null)
    {
        Destroy(playerController);
    }
    Boss1Attack bossAttack = FindObjectOfType<Boss1Attack>();
    if (bossAttack != null)
    {
        Destroy(bossAttack);
    }

    PlayerAttackAnimation playerAttackAnimation = GetComponent<PlayerAttackAnimation>();
    if (playerAttackAnimation != null)
    {
        Destroy(playerAttackAnimation);
    }
        if (bossAttack != null)
        {
            bossAttack.StopTargetingPlayer();
        }
    }
}
