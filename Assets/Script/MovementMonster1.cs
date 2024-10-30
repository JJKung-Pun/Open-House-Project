using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMonster1 : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed at which the monster moves
    public float stopDistance = 1.5f; // Distance to stop from the player
    private Transform player; // Reference to the player's position
    private Rigidbody2D rb; // Monster's Rigidbody2D

    void Start()
    {
        // Get the player object and Rigidbody2D
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null)
        {
            MoveTowardPlayer();
        }
    }

    // Function to check if the monster is moving
    public bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1f;
    }

    private void MoveTowardPlayer()
    {
        // Calculate the distance between the monster and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Move toward the player if the distance is greater than the stopDistance
        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed; // Apply movement
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving when in range
        }
    }
}
