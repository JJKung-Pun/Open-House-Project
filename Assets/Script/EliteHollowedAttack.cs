using UnityEngine;

public class EliteHollowedAttack : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 2.5f;
    [SerializeField] private int damage = 40;
    [SerializeField] private GameObject hitbox;

    private EliteHollowedMove moveScript;
    private float cooldownTimer = 0f;
    private bool isCooldown = false;
    private bool playerInRange = false;

    void Start()
    {
        moveScript = GetComponent<EliteHollowedMove>();
        if (hitbox != null)
        {
            hitbox.SetActive(false); // Ensure hitbox starts inactive
        }
    }

    void Update()
    {
        if (!isCooldown && playerInRange)
        {
            Attack();
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                moveScript.enabled = true;
            }
        }
    }

    void Attack()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(true); // Activate hitbox for the attack
            Invoke(nameof(DeactivateHitbox), 1f); // Deactivate hitbox after 1 second
        }

        PlayerHP playerHP = FindPlayerInHitbox();
        if (playerHP != null)
        {
            playerHP.TakeDamage(damage);
        }

        isCooldown = true;
        cooldownTimer = cooldownTime;
        moveScript.enabled = false; // Disable movement during cooldown
    }

    void DeactivateHitbox()
    {
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }

    PlayerHP FindPlayerInHitbox()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(hitbox.transform.position, hitbox.GetComponent<BoxCollider2D>().size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return collider.GetComponent<PlayerHP>();
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
