using UnityEngine;

public class HollowedSoldierMove : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 3f;
    public float stopRange = 0.5f;
    public float moveSpeed = 2f;
    public HollowedSoldierAttack attackScript;  // Reference to the HollowedSoldierAttack script
    private Animator animator;  // Reference to the Animator component

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D attackHitbox;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();  // Get the Animator component

        // Get the attack hitbox (ensure you assign the correct child with name "AHitBox" in the Inspector)
        attackHitbox = transform.Find("AHitBox")?.GetComponent<BoxCollider2D>();

        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox (AHitBox) is missing! Please assign it.");
        }

        if (attackScript == null)
        {
            attackScript = GetComponent<HollowedSoldierAttack>(); // Automatically assign the attack script if not manually set
        }
    }

    void Update()
    {
        if (player == null || attackHitbox == null || attackScript == null) return;

        // Check if attack is on cooldown, if so stop movement
        if (attackScript.IsOnCooldown()) return;

        float distanceToPlayerSqr = (transform.position - player.position).sqrMagnitude; // Use squared distance for comparison

        // Check if the player is within detection range
        if (distanceToPlayerSqr <= detectionRange * detectionRange)
        {
            MoveTowardsPlayer(distanceToPlayerSqr);
            AdjustHitboxPosition();

            // Trigger walking animation when moving
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop walking animation when out of range
            animator.SetBool("isWalking", false);
        }
    }

    private void MoveTowardsPlayer(float distanceToPlayerSqr)
    {
        if (distanceToPlayerSqr > stopRange * stopRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void AdjustHitboxPosition()
    {
        // Determine whether to flip the sprite based on the player's position
        bool shouldFlip = player.position.x < transform.position.x;
        if (spriteRenderer.flipX != shouldFlip)
        {
            spriteRenderer.flipX = shouldFlip;
        }

        // Adjust the hitbox position based on the sprite flip
        Vector2 hitboxPosition = attackHitbox.transform.localPosition;
        hitboxPosition.x = shouldFlip ? -Mathf.Abs(hitboxPosition.x) : Mathf.Abs(hitboxPosition.x);
        attackHitbox.transform.localPosition = hitboxPosition;
    }

    // Call this method to trigger the attack animation
    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("isAttacking");  // Trigger the attack animation
    }

    // Call this method to trigger the damaged animation
    public void TriggerDamagedAnimation()
    {
        animator.SetTrigger("isDamaged");  // Trigger the damage animation
    }
}
