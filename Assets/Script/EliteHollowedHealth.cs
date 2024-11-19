using UnityEngine;

public class EliteHollowedHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth; // Now visible in the inspector

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Enemy Health initialized: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
