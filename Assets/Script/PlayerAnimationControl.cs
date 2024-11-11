using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component

    void Update()
    {
        // If the player presses A (left) or D (right) key, start the walking animation
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);  // Ensure it's not in the running state
        }
        // If neither A nor D is pressed, stop the walking animation (back to idle)
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Trigger the dash animation when the player presses the Space key
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
