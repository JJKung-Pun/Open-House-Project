using UnityEngine;

public class HollowedSoldierAttack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1f;
    public int damage = 20;
    public float attackCooldown = 3f;
    public float hitboxActiveTime = 1f;  // Duration for which the hitbox is active
    public BoxCollider2D attackHitbox;
    public SpriteRenderer hitboxSpriteRenderer;
    public Animator animator;  // Animator component for triggering attack animations

    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        if (player == null || attackHitbox == null) return;

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

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
            isOnCooldown = true;
            cooldownTimer = attackCooldown;
        }
    }

    private void InitializeComponents()
    {
        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox is missing. Please assign it in the Inspector.");
            return;
        }

        if (hitboxSpriteRenderer == null)
        {
            hitboxSpriteRenderer = attackHitbox.GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator is missing. Please assign it in the Inspector or add it to the GameObject.");
            }
        }

        attackHitbox.enabled = false;
        if (hitboxSpriteRenderer != null)
        {
            hitboxSpriteRenderer.enabled = false;
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("isAttacking");  // Trigger the attack animation
        }

        if (attackHitbox == null) return;

        // Enable the attack hitbox
        attackHitbox.enabled = true;

        // Make the hitbox visible
        if (hitboxSpriteRenderer != null)
        {
            hitboxSpriteRenderer.enabled = true;
        }

        // Detect collision with the player using LayerMask for optimization
        LayerMask playerLayer = LayerMask.GetMask("Player");
        Collider2D hit = Physics2D.OverlapBox(attackHitbox.bounds.center, attackHitbox.bounds.size, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            PlayerHP playerHealth = hit.GetComponent<PlayerHP>();
            playerHealth?.TakeDamage(damage);
        }

        // Disable the hitbox after the active time
        Invoke("DisableHitbox", hitboxActiveTime);
    }

    void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }

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
