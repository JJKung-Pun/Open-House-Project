using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Movement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f; 
    public float stopDistance = 0.5f;

    private float fixedY;
    private bool canMove = true;
    private Boss1HP bossHealth;
    private Animator animator;

    void Start()
    {
        fixedY = transform.position.y;
        bossHealth = GetComponent<Boss1HP>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove || (bossHealth != null && bossHealth.currentHealth <= 1)) // Update this line
        {
            SetWalkingAnimation(false);
            return;
        }

        MoveTowardsPlayer();
    }


    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > stopDistance)
            {
                Vector2 targetPosition = new Vector2(player.transform.position.x, fixedY);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                SetWalkingAnimation(true);
                
                // Flip the boss if the player is behind it
                FlipIfNeeded();
            }
            else
            {
                SetWalkingAnimation(false);
            }
        }
    }

    private void FlipIfNeeded()
    {
        // Calculate the direction to the player
        Vector2 directionToPlayer = player.transform.position - transform.position;

        // Check if the player is behind the boss (i.e., negative direction on the x-axis)
        if (directionToPlayer.x < 0 && transform.localScale.x > 0)
        {
            Flip(); // Flip the boss to face the left
        }
        else if (directionToPlayer.x > 0 && transform.localScale.x < 0)
        {
            Flip(); // Flip the boss to face the right
        }
    }

    private void Flip()
    {
        // Flip the scale of the boss along the x-axis
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Invert the x-scale
        transform.localScale = localScale;
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
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

    // New method to stop attacking
    public void StopAttacking()
    {
        // Here you can set an animator trigger or bool to stop any attack animations
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false); // Ensure you have an "IsAttacking" parameter in your Animator
        }
        // You might want to implement additional logic to stop any attack-related behavior
    }
}
