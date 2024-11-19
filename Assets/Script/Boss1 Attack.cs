using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    public GameObject WideSweepingStrikes1;
    public GameObject WideSweepingStrikes2;
    public GameObject OverheadSlamsHitbox;
    public GameObject ChainSweepsHitbox;
    public Animator bossAnimator;

    public int wideSweepingStrikes1Damage = 20;
    public int wideSweepingStrikes2Damage = 25;
    public int overheadSlamsDamage = 50;
    public int chainSweepsDamage = 20;

    public float attackCooldown = 5.0f; // Cooldown between attacks
    public float rangeToPlayer = 5.0f;

    private bool isPlayerInRange = false;
    private bool isAttacking = false;
    private bool isCooldownActive = false; // Track cooldown state
    private Boss1Movement bossMovement;
    private Boss1HP bossHealth;

    private bool wideSweepingStrikes1Used = false;
    private bool wideSweepingStrikes2Used = false;
    private bool overheadSlamsUsed = false;
    private bool chainSweepsUsed = false;

    private void Start()
    {
        bossMovement = GetComponent<Boss1Movement>();
        bossHealth = GetComponent<Boss1HP>();
        DisableAllHitboxes();
    }

    private void Update()
    {
        if (bossHealth != null && bossHealth.currentHealth <= 0)
        {
            isAttacking = false;
            return;
        }

        if (isCooldownActive) return; // If cooldown is active, skip other updates

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
        isCooldownActive = true; // Start cooldown immediately after an attack begins

        if (bossMovement != null)
        {
            bossMovement.StopMovement(); // Stop boss movement during attack
        }

        float randomValue = Random.value;

        if (!overheadSlamsUsed && randomValue < 0.5f)
        {
            OverheadSlams();
        }
        else if (!wideSweepingStrikes1Used && randomValue < 0.75f)
        {
            WideSweepingStrikes1Attack();
        }
        else if (!chainSweepsUsed)
        {
            ChainSweeps();
        }

        yield return new WaitForSeconds(attackCooldown); // Apply cooldown between attacks

        ResetAttackUsages();

        if (bossMovement != null)
        {
            bossMovement.StartMovement(); // Resume movement after cooldown
        }

        isCooldownActive = false; // End cooldown after it's done
        isAttacking = false;
    }

    private void ChainSweeps()
    {
        ChainSweepsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("ChainSweeps");
        }

        ApplyDamageWithHitbox(ChainSweepsHitbox, chainSweepsDamage);
        StartCoroutine(DisableHitboxAfterDelay(ChainSweepsHitbox));
        chainSweepsUsed = true;
    }

    private void WideSweepingStrikes1Attack()
    {
        WideSweepingStrikes1.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes1");
        }

        ApplyDamageWithHitbox(WideSweepingStrikes1, wideSweepingStrikes1Damage);

        // Wait for WideSweepingStrikes1 to finish, then use WideSweepingStrikes2
        wideSweepingStrikes1Used = true;
        StartCoroutine(DisableHitboxAfterDelay(WideSweepingStrikes1, 0.5f)); // 0.5 seconds duration for WideSweepingStrikes1

        StartCoroutine(WideSweepingStrikes2Attack());
    }

    private IEnumerator WideSweepingStrikes2Attack()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure the first attack finishes

        WideSweepingStrikes2.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes2");
        }

        ApplyDamageWithHitbox(WideSweepingStrikes2, wideSweepingStrikes2Damage);

        wideSweepingStrikes2Used = true;
        StartCoroutine(DisableHitboxAfterDelay(WideSweepingStrikes2, 0.5f)); // 0.5 seconds duration for WideSweepingStrikes2
    }

    private void OverheadSlams()
    {
        OverheadSlamsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OverheadSlams");
        }

        ApplyDamageWithHitbox(OverheadSlamsHitbox, overheadSlamsDamage);
        StartCoroutine(DisableHitboxAfterDelay(OverheadSlamsHitbox));
        overheadSlamsUsed = true;
    }

    private void ApplyDamageWithHitbox(GameObject hitbox, int damage)
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

    private IEnumerator DisableHitboxAfterDelay(GameObject hitbox, float duration = 1f)
    {
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        hitbox.SetActive(false); // Deactivate hitbox after the delay
    }

    private void DisableAllHitboxes()
    {
        WideSweepingStrikes1.SetActive(false);
        WideSweepingStrikes2.SetActive(false);
        OverheadSlamsHitbox.SetActive(false);
        ChainSweepsHitbox.SetActive(false);
    }

    private void ResetAttackUsages()
    {
        wideSweepingStrikes1Used = false;
        wideSweepingStrikes2Used = false;
        overheadSlamsUsed = false;
        chainSweepsUsed = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (WideSweepingStrikes1.activeSelf)
        {
            Gizmos.DrawWireCube(WideSweepingStrikes1.transform.position, WideSweepingStrikes1.GetComponent<BoxCollider2D>().size);
        }
        if (WideSweepingStrikes2.activeSelf)
        {
            Gizmos.DrawWireCube(WideSweepingStrikes2.transform.position, WideSweepingStrikes2.GetComponent<BoxCollider2D>().size);
        }
        if (OverheadSlamsHitbox.activeSelf)
        {
            Gizmos.DrawWireCube(OverheadSlamsHitbox.transform.position, OverheadSlamsHitbox.GetComponent<BoxCollider2D>().size);
        }
        if (ChainSweepsHitbox.activeSelf)
        {
            Gizmos.DrawWireCube(ChainSweepsHitbox.transform.position, ChainSweepsHitbox.GetComponent<BoxCollider2D>().size);
        }
    }
}
