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
        if (randomValue < 0.33f)
        {
            yield return OverheadSlams();
        }
        else if (randomValue < 0.66f)
        {
            yield return ChainSweeps();
        }
        else
        {
            yield return WideSweepingStrikes();
        }

        yield return new WaitForSeconds(attackCooldown);

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
            bossAnimator.SetTrigger("ChainSweeps"); // Trigger specific animation
        }

        yield return new WaitForSeconds(damageDelay);
        
        // Damage logic
        float remainingDuration = hitboxDuration - damageDelay;
        float damageInterval = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < remainingDuration)
        {
            ApplyDamageToPlayer(ChainSweepsHitbox, chainSweepsDamage);
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

        ChainSweepsHitbox.SetActive(false);
    }

    private IEnumerator WideSweepingStrikes()
    {
        WideSweepingStrikes1.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes1"); // Trigger animation for first strike
        }

        yield return new WaitForSeconds(damageDelay);
        
        // Damage logic for the first strike
        float remainingDuration = hitboxDuration - damageDelay;
        float damageInterval = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < remainingDuration)
        {
            ApplyDamageToPlayer(WideSweepingStrikes1, wideSweepingStrikesDamage);
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

        WideSweepingStrikes1.SetActive(false);

        WideSweepingStrikes2.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes2"); // Trigger animation for second strike
        }

        yield return new WaitForSeconds(damageDelay);
        
        // Damage logic for the second strike
        elapsedTime = 0f;

        while (elapsedTime < remainingDuration)
        {
            ApplyDamageToPlayer(WideSweepingStrikes2, wideSweepingStrikesDamage);
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

        WideSweepingStrikes2.SetActive(false);
    }

    private IEnumerator OverheadSlams()
    {
        OverheadSlamsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OverheadSlams"); // Trigger overhead slam animation
        }

        yield return new WaitForSeconds(damageDelay);
        
        // Damage logic
        float remainingDuration = hitboxDuration - damageDelay;
        float damageInterval = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < remainingDuration)
        {
            ApplyDamageToPlayer(OverheadSlamsHitbox, overheadSlamsDamage);
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval);
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
                    playerHP.TakeDamage(damage);
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
