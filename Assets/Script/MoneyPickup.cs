using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public int moneyAmount = 1; // Amount of money this collectible gives
    private MoneyManager moneyManager; // Reference to the MoneyManager script

    private void Start()
    {
        // Find the MoneyManager in the scene
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the colliding object is the player
        {
            moneyManager.AddMoney(moneyAmount); // Add money to the player's total
            Destroy(gameObject); // Destroy the collectible object
        }
    }
}
