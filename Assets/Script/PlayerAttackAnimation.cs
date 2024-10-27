using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    public Animator animator;
    public float comboResetTime = 1.0f;  // Time window to detect multiple presses

    private bool isAttacking = false;
    private float lastAttackTime;
    private int attackCount = 0;

    void Update()
    {
        // Detect key press (J key)
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Increment the attack count
            if (!isAttacking)
            {
                // Start a new attack sequence
                attackCount = 1;  // Reset to 1 for the first attack
                lastAttackTime = Time.time;  // Record the time of the attack
                Debug.Log("Attack Count: " + attackCount);
                Attack();
            }
            else
            {
                // Check if within the combo reset time to increment attack count
                if (Time.time - lastAttackTime <= comboResetTime)
                {
                    attackCount++;
                    Debug.Log("Attack Count Incremented: " + attackCount);
                }
                else
                {
                    // If combo time is exceeded, reset count and start new attack
                    attackCount = 1;  // Start over
                    lastAttackTime = Time.time;  // Update last attack time
                    Debug.Log("Combo Reset. Attack Count: " + attackCount);
                    Attack();  // Call Attack for the new sequence
                }
            }
        }

        // Reset attack count if the combo reset time has passed since last attack
        if (isAttacking && Time.time - lastAttackTime > comboResetTime)
        {
            Debug.Log("Resetting Attack Count.");
            attackCount = 0;  // Reset attack count
            isAttacking = false; // Allow new attack
        }
    }

    void Attack()
    {
        if (attackCount == 1)
        {
            Debug.Log("Playing single press sequence: PlayerAttackStart -> PlayerAttackEnd");
            StartCoroutine(PlayAttackSequence(new string[] { "PlayerAttackStart", "PlayerAttackEnd" }));
        }
        else if (attackCount > 1)
        {
            Debug.Log("Playing multiple press sequence: PlayerAttackStart -> PlayerAttackLoop -> PlayerAttackEnd");
            StartCoroutine(PlayAttackSequence(new string[] { "PlayerAttackStart", "PlayerAttackLoop", "PlayerAttackEnd" }));
        }

        isAttacking = true; // Set attacking state
    }

    private System.Collections.IEnumerator PlayAttackSequence(string[] animations)
    {
        // Play each animation in sequence with a delay
        foreach (string anim in animations)
        {
            Debug.Log("Playing animation: " + anim);
            animator.Play(anim);
            yield return new WaitForSeconds(0.75f);  // Wait for animation to complete
        }

        // After finishing animations, reset states
        isAttacking = false;  
        attackCount = 0;      // Reset attack count
    }
}
