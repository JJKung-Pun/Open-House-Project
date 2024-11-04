using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f; 
    public float stopDistance = 0.5f;

    private float fixedY;
    private bool canMove = true; // Controls whether the enemy can move
    private Boss1HP bossHealth; // Reference to the boss's health script
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        fixedY = transform.position.y;
        bossHealth = GetComponent<Boss1HP>(); // Get the boss health component
        animator = GetComponent<Animator>(); // Get the Animator component

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the boss GameObject.");
        }
    }

    void Update()
    {
        // Check health and movement ability
        if (!canMove || bossHealth.currentHealth <= 0)
        {
            SetWalkingAnimation(false); // Stop walking animation
            return; // Exit Update to stop further execution
        }

        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log($"Distance to player: {distance}"); // Debug log

            if (distance > stopDistance)
            {
                Vector2 targetPosition = new Vector2(player.transform.position.x, fixedY);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                SetWalkingAnimation(true); // Set walking animation
            }
            else
            {
                SetWalkingAnimation(false); // Stop walking animation if within stop distance
            }
        }
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
            Debug.Log($"Setting walking animation to: {isWalking}"); // Debug log
        }
        else
        {
            Debug.LogError("Animator is null; walking animation cannot be set.");
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
