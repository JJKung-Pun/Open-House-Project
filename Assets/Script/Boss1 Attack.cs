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

    public float hitboxDuration = 2.5f; 
    public float attackCooldown = 5.0f; 
    public float rangeToPlayer = 5.0f; 
    public float damageDelay = 0.5f; 

    private bool isPlayerInRange = false; 
    private bool isAttacking = false; 
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
        if (bossHealth != null && bossHealth.currentHealth <= 1)
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

        if (!overheadSlamsUsed && randomValue < 0.5f)
        {
            yield return OverheadSlams();
        }
        else if (!wideSweepingStrikes1Used && randomValue < 0.75f)
        {
            yield return WideSweepingStrikes1Attack();
            yield return WideSweepingStrikes2Attack(); // WSS2 follows immediately after WSS1
        }
        else if (!chainSweepsUsed)
        {
            yield return ChainSweeps();
        }

        yield return new WaitForSeconds(attackCooldown);

        ResetAttackUsages();

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

        yield return new WaitForSeconds(damageDelay);
        yield return ApplyDamageWithHitbox(ChainSweepsHitbox, chainSweepsDamage);
        ChainSweepsHitbox.SetActive(false);
        chainSweepsUsed = true;
    }

    private IEnumerator WideSweepingStrikes1Attack()
    {
        WideSweepingStrikes1.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("WideSweepingStrikes1");
        }

        yield return new WaitForSeconds(damageDelay);
        yield return ApplyDamageWithHitbox(WideSweepingStrikes1, wideSweepingStrikes1Damage);
        WideSweepingStrikes1.SetActive(false);
        wideSweepingStrikes1Used = true;
    }

    private IEnumerator WideSweepingStrikes2Attack()
    {
        if (wideSweepingStrikes1Used) // Only perform WSS2 if WSS1 was used
        {
            WideSweepingStrikes2.SetActive(true);
            if (bossAnimator != null)
            {
                bossAnimator.SetTrigger("WideSweepingStrikes2");
            }

            yield return new WaitForSeconds(damageDelay);
            yield return ApplyDamageWithHitbox(WideSweepingStrikes2, wideSweepingStrikes2Damage);
            WideSweepingStrikes2.SetActive(false);
            wideSweepingStrikes2Used = true;
        }
    }

    private IEnumerator OverheadSlams()
    {
        OverheadSlamsHitbox.SetActive(true);
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OverheadSlams");
        }

        yield return new WaitForSeconds(damageDelay);
        yield return ApplyDamageWithHitbox(OverheadSlamsHitbox, overheadSlamsDamage);
        OverheadSlamsHitbox.SetActive(false);
        overheadSlamsUsed = true;
    }

    private IEnumerator ApplyDamageWithHitbox(GameObject hitbox, int damage)
    {
        BoxCollider2D boxCollider = hitbox.GetComponent<BoxCollider2D>();
        if (boxCollider == null) yield break;

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

        yield return new WaitForSeconds(hitboxDuration - damageDelay);
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
