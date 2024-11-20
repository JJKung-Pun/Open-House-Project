using UnityEngine;

public class HollowedSoldierHealth : MonoBehaviour
{
    public int maxHealth = 15;
    public int currentHealth;
    public Animator animator;  // Reference to the Animator

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();  // Ensure Animator is attached to the same GameObject
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("takeDamage");  // Trigger the TakeDamage animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");  // Trigger the death animation
        Destroy(gameObject, 1f);  // Destroy after a delay (adjust time as needed)
    }
}
