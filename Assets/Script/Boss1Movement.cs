using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f; 
    public float stopDistance = 0.5f;

    private float distance;
    private float fixedY;
    private bool canMove = true; // Controls whether the enemy can move
    private Boss1HP bossHealth; // Reference to the boss's health script

    void Start()
    {
        fixedY = transform.position.y;
        bossHealth = GetComponent<Boss1HP>(); // Get the boss health component
    }

    void Update()
    {
        if (bossHealth.currentHealth <= 1) // Check if health is 1 or less
        {
            canMove = false; // Stop movement
            return; // Exit Update to stop further execution
        }

        if (player != null && canMove)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > stopDistance)
            {
                Vector2 targetPosition = new Vector2(player.transform.position.x, fixedY);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void StartMovement()
    {
        canMove = true;
    }
}
