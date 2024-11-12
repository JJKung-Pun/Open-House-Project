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
        if (!canMove || (bossHealth != null && bossHealth.currentHealth <= 1))
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
                
                FlipIfNeeded(); // Flip the boss if needed to face the player
            }
            else
            {
                SetWalkingAnimation(false);
            }
        }
    }

    private void FlipIfNeeded()
    {
        float directionToPlayer = player.transform.position.x - transform.position.x;

        if ((directionToPlayer < 0 && transform.localScale.x > 0) || (directionToPlayer > 0 && transform.localScale.x < 0))
        {
            Flip(); // Flip only if the boss is facing the wrong direction
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        
        Debug.Log("Flipped Boss. Now facing " + (transform.localScale.x > 0 ? "right" : "left"));
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

    public void StopAttacking()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
        }
    }
}
