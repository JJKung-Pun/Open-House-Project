using UnityEngine;

public class HollowedSoldierMove : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 3f;
    public float stopRange = 0.5f;
    public float moveSpeed = 2f;
    public HollowedSoldierAttack attackScript;  // Reference to the HollowedSoldierAttack script

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D attackHitbox;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > stopRange)
            {
                // Calculate the direction towards the player
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }

            // Flip the sprite based on the player's position relative to the enemy
            bool shouldFlip = player.position.x < transform.position.x;
            spriteRenderer.flipX = shouldFlip;

            // Adjust the position of the attack hitbox based on the sprite flip
            Vector2 hitboxPosition = attackHitbox.transform.localPosition;

            if (shouldFlip)
            {
                // Move the hitbox to the left side when the sprite is flipped
                hitboxPosition.x = -Mathf.Abs(hitboxPosition.x);  // Adjust position to the left
            }
            else
            {
                // Move the hitbox to the right side when the sprite is not flipped
                hitboxPosition.x = Mathf.Abs(hitboxPosition.x);  // Adjust position to the right
            }

            // Apply the new position to the attack hitbox
            attackHitbox.transform.localPosition = hitboxPosition;
        }
    }
}
