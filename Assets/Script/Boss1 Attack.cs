using System.Collections;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    public GameObject WideSweepingStrikes1;
    public GameObject WideSweepingStrikes2;
    public GameObject OverheadSlamsHitbox;
    public GameObject ChainSweepsHitbox;
    public Animator bossAnimator; // Animator for handling animations

    public int wideSweepingStrikesDamage = 35;
    public int overheadSlamsDamage = 45;
    public int chainSweepsDamage = 30;

    public float hitboxDuration = 2.5f; // Total time for hitbox, includes delay before damage
    public float attackCooldown = 5.0f; 
    public float rangeToPlayer = 5.0f; 
    public float damageDelay = 0.5f; // Adjusted time before damage is applied

    private bool isPlayerInRange = false; 
    private bool isAttacking = false; 
    private Boss1Movement bossMovement; 
    private Boss1HP bossHealth;

    private void Start()
    {
        bossMovement = GetComponent<Boss1Movement>();
        bossHealth = GetComponent<Boss1HP>();
        DisableAllHitboxes();
        bossAnimator = GetComponent<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("ChainSweeps"); // Test trigger
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }
    }

    private void Update()
    {
        if (bossHealth.currentHealth <= 1)
        {
            isAttacking = false;
            return;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            isPlayerInRange = distanceToPlayer <= rangeToPlayer;

            if (isPlayerInRange && !isAttacking)
            {
                StartCoroutine(SelectRandomAttack());
            }
        }
    }

    private IEnumerator SelectRandomAttack()
    {
        isAttacking = true;

        if (bossMovement != null)
        {
            bossMovement.StopMovement();
        }

        float randomValue = Random.value;
        float attackDuration = 0f; // Variable to track the overall duration of the attack

        if (randomValue < 0.33f)
        {
            attackDuration = 1.5f; // Adjust according to OverheadSlams duration
            yield return OverheadSlams();
        }
        else if (randomValue < 0.66f)
        {
            attackDuration = 2.5f; // Adjust according to ChainSweeps duration
            yield return ChainSweeps();
        }
        else
        {
            attackDuration = 3f; // Adjust according to WideSweepingStrikes duration
            yield return WideSweepingStrikes();
        }

        yield return new WaitForSeconds(attackCooldown - attackDuration); // Wait for cooldown minus attack duration

        if (bossMovement != null)
        {
            bossMovement.StartMovement();
        }

        isAttacking = false;
    }

    private IEnumerator ChainSweeps()
    {
        ChainSweepsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("ChainSweeps");
        }

        float animationDuration = 1.0f;  // Set this to the actual animation length
        yield return new WaitForSeconds(animationDuration); // Wait for animation to finish

        yield return new WaitForSeconds(damageDelay); // Delay before damage application

        // Now apply damage while the hitbox is active
        float elapsedTime = 0f;
        while (elapsedTime < hitboxDuration)
        {
            ApplyDamageToPlayer(ChainSweepsHitbox, chainSweepsDamage);
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f); // Wait for the next damage frame
        }

        ChainSweepsHitbox.SetActive(false);
    }

    private IEnumerator WideSweepingStrikes()
    {
        WideSweepingStrikes1.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes1");
        }

        yield return new WaitForSeconds(damageDelay);

        // Apply damage for the first strike
        float elapsedTime = 0f;
        while (elapsedTime < hitboxDuration)
        {
            ApplyDamageToPlayer(WideSweepingStrikes1, wideSweepingStrikesDamage);
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        WideSweepingStrikes1.SetActive(false);

        // Second strike with a delay
        yield return new WaitForSeconds(0.3f); // Small gap between strikes

        WideSweepingStrikes2.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes2");
        }

        yield return new WaitForSeconds(damageDelay);

        // Apply damage for the second strike
        elapsedTime = 0f;
        while (elapsedTime < hitboxDuration)
        {
            ApplyDamageToPlayer(WideSweepingStrikes2, wideSweepingStrikesDamage);
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        WideSweepingStrikes2.SetActive(false);
    }

    private IEnumerator OverheadSlams()
    {
        OverheadSlamsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OverheadSlams");
        }

        float animationDuration = 1.5f; // Set this to the actual animation length
        yield return new WaitForSeconds(animationDuration); // Wait for animation to finish

        yield return new WaitForSeconds(damageDelay); // Delay before damage application

        // Apply damage while the hitbox is active
        float elapsedTime = 0f;
        while (elapsedTime < hitboxDuration)
        {
            ApplyDamageToPlayer(OverheadSlamsHitbox, overheadSlamsDamage);
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        OverheadSlamsHitbox.SetActive(false);
    }

    private void ApplyDamageToPlayer(GameObject hitbox, int damage)
    {
        BoxCollider2D boxCollider = hitbox.GetComponent<BoxCollider2D>();
        if (boxCollider == null) return;

        Vector2 size = boxCollider.size;
        Vector2 position = hitbox.transform.position;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(position, size, 0);
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerHP playerHP = collider.GetComponent<PlayerHP>();
                if (playerHP != null)
                {
                    playerHP.TakeDamage(damage); // Apply damage
                }
            }
        }
    }

    private void DisableAllHitboxes()
    {
        WideSweepingStrikes1.SetActive(false);
        WideSweepingStrikes2.SetActive(false);
        OverheadSlamsHitbox.SetActive(false);
        ChainSweepsHitbox.SetActive(false);
    }

    public void StopTargetingPlayer()
    {
        isPlayerInRange = false;
        isAttacking = false;
        if (bossMovement != null)
        {
            bossMovement.StopMovement();
        }
    }
}
