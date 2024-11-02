using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Attack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public int damageAmount = 10;
    public GameObject hitboxPrefab; // Prefab for the hitbox
    public float hitboxDuration = 0.5f; // Duration to show hitbox (adjusted)

    private float lastAttackTime;
    private Animator animator;
    private GameObject currentHitbox;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        lastAttackTime = Time.time;

        // Trigger the attack animation
        MonsterAnimationControl animationControl = GetComponent<MonsterAnimationControl>();
        if (animationControl != null)
        {
            animationControl.TriggerAttack(); // Use the animation control to trigger the attack animation
        }

        ShowHitbox(); // Show the hitbox immediately when the attack starts
    }

    // Call this function via an animation event during the attack animation
    public void ShowHitbox()
    {
        if (currentHitbox != null) Destroy(currentHitbox);
        currentHitbox = Instantiate(hitboxPrefab, transform.position, Quaternion.identity);
        currentHitbox.transform.SetParent(transform); // Parent to monster for positioning
        currentHitbox.SetActive(true);
        StartCoroutine(DeactivateHitboxAfterDelay());
        PerformAttack(); // Perform attack immediately after showing the hitbox
    }

    private IEnumerator DeactivateHitboxAfterDelay()
    {
        yield return new WaitForSeconds(hitboxDuration);
        if (currentHitbox != null)
        {
            Destroy(currentHitbox); // Properly destroy the hitbox
            currentHitbox = null;   // Reset current hitbox reference
        }
    }

    // This function checks for damage
    public void PerformAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            PlayerHP playerHealth = player.GetComponent<PlayerHP>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    // This function should be called at the end of the attack animation
    public void EndAttack()
    {
        // This can be used to reset any flags or perform additional logic at the end of the attack
    }
}
