using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationControl : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    private MovementMonster1 movementScript; // Reference to the movement script

    void Start()
    {
        // Get references to the Animator and movement script
        animator = GetComponent<Animator>();
        movementScript = GetComponent<MovementMonster1>();
    }

    void Update()
    {
        // Check if the monster is moving or idle
        if (movementScript != null)
        {
            if (movementScript.IsMoving()) // Check if the monster is moving
            {
                animator.SetBool("isWalking", true); // Play walking animation
                animator.SetBool("isIdle", false); // Stop idle animation
            }
            else
            {
                animator.SetBool("isWalking", false); // Stop walking animation
                animator.SetBool("isIdle", true); // Play idle animation
            }
        }
    }
}
