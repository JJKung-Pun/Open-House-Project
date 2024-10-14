using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMonster1 : MonoBehaviour
{
    public float moveSpeed = 2.0f; 
    public float stopDistance = 1.5f; 

    private Transform player; 
    private Rigidbody2D rb; 
    private Monster1Attack attackScript; // Reference to the Monster1Attack script

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; 
        rb = GetComponent<Rigidbody2D>();
        attackScript = GetComponent<Monster1Attack>(); // Reference the attack script
    }

    private void Update()
    {
        if (player != null)
        {
            MoveTowardPlayer();
        }
    }

    private void MoveTowardPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > stopDistance && !attackScript.isAttacking) 
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed; 
        }
        else
        {
            rb.velocity = Vector2.zero; 
        }
    }
}
