using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public GameObject youDiedPanel;
    private Boss1Attack bossAttack;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        youDiedPanel.SetActive(false);
        bossAttack = FindObjectOfType<Boss1Attack>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        youDiedPanel.SetActive(true);
        if (bossAttack != null)
        {
            bossAttack.StopTargetingPlayer();
        }
    }
}
