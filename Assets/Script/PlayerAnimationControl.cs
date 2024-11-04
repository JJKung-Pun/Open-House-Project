using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    private Rigidbody2D rb;    // Reference to the player's Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D attached to the player
    }

    void Update()
    {
        // Get the player's current speed (magnitude of velocity)
        float speed = rb.velocity.magnitude;

        // If the player is moving (speed > 0.5) and holding Shift (for running)
        if (speed > 5.5f && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        // If the player is moving but not running (speed between 0.5 and 5.5)
        else if (speed > 0.5f)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        // If the player is not moving
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);  // Go back to idle
        }
          if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerDashAnimation();
        }
    }

    private void TriggerDashAnimation()
    {
        animator.SetTrigger("Dash"); // Use a trigger to start the dash animation
    }
}
