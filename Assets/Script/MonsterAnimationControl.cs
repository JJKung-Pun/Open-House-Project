using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationControl : MonoBehaviour
{
    private Animator animator;
    private MovementMonster1 movementScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementScript = GetComponent<MovementMonster1>();
    }

    void Update()
    {
        if (movementScript != null)
        {
            if (movementScript.IsMoving) // Access IsMoving as a property
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
            }
        }
    }

    // Call this method to trigger the attack animation
    public void TriggerAttack()
    {
        animator.SetTrigger("isAttack"); // Ensure this matches your parameter name in the Animator
    }
}
