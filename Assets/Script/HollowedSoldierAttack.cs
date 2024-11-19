using UnityEngine;

public class HollowedSoldierAttack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1f;
    public int damage = 20;
    public float attackCooldown = 3f;  // Changed to 3 seconds cooldown
    public BoxCollider2D attackHitbox;  // The BoxCollider2D for the attack hitbox, assigned in the Inspector
    public SpriteRenderer hitboxSpriteRenderer;  // SpriteRenderer to make the hitbox visible

    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    void Start()
    {
        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox is missing. Please assign it in the Inspector.");
        }
        if (hitboxSpriteRenderer == null)
        {
            // Try to find the SpriteRenderer in the hitbox if not assigned
            hitboxSpriteRenderer = attackHitbox.GetComponent<SpriteRenderer>();
        }
        
        attackHitbox.enabled = false;  // Initially disable the hitbox
        if (hitboxSpriteRenderer != null)
        {
            hitboxSpriteRenderer.enabled = false;  // Initially make the hitbox invisible
        }
    }

    void Update()
    {
        if (player == null || attackHitbox == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            Attack();
            isOnCooldown = true;
            cooldownTimer = attackCooldown;
        }
    }

    void Attack()
    {
        // Enable the attack hitbox (BoxCollider2D) to detect collisions
        attackHitbox.enabled = true;

        // Make the hitbox visible for 1 second
        if (hitboxSpriteRenderer != null)
        {
            hitboxSpriteRenderer.enabled = true;
        }

        // Check if the hitbox collides with the player
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackHitbox.bounds.center, attackHitbox.bounds.size, 0f);

        foreach (Collider2D hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHP playerHealth = hit.GetComponent<PlayerHP>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }

        // After 1 second, disable the hitbox and make it invisible
        Invoke("DisableHitbox", 1f);  // Hitbox lasts for 1 second
    }

    void DisableHitbox()
    {
        // Disable the attack hitbox and make it invisible
        attackHitbox.enabled = false;
        if (hitboxSpriteRenderer != null)
        {
            hitboxSpriteRenderer.enabled = false;
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
