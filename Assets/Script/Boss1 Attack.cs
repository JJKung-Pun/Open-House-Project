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

    public float hitboxDuration = 2f; // Total time for hitbox, includes delay before damage
    public float attackCooldown = 5.0f;
    public float rangeToPlayer = 5.0f;
    public float damageDelay = 0.2f; // Adjusted time before damage is applied

    // Unique pre-attack delays for each attack type
    public float chainSweepsDelay = 1.0f;
    public float wideSweepingStrikes1Delay = 1.5f;
    public float wideSweepingStrikes2Delay = 1.0f; // Added specific delay for second wide strike
    public float overheadSlamsDelay = 2.0f;

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
        float attackDuration = 0f;

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

        yield return new WaitForSeconds(attackCooldown - attackDuration);

        if (bossMovement != null)
        {
            bossMovement.StartMovement();
        }

        isAttacking = false;
    }

    private IEnumerator ChainSweeps()
    {
        yield return new WaitForSeconds(chainSweepsDelay); // Unique delay for ChainSweeps

        ChainSweepsHitbox.SetActive(true);
        TriggerAnimation("ChainSweeps");

        yield return new WaitForSeconds(damageDelay);

        ApplyDamageToPlayer(ChainSweepsHitbox, chainSweepsDamage);

        yield return new WaitForSeconds(hitboxDuration);

        ChainSweepsHitbox.SetActive(false);
    }

    private IEnumerator WideSweepingStrikes()
    {
        // First strike
        yield return new WaitForSeconds(wideSweepingStrikes1Delay); // Unique delay for Wide Sweeping Strikes 1

        WideSweepingStrikes1.SetActive(true);
        TriggerAnimation("WideSweepingStrikes1");

        yield return new WaitForSeconds(damageDelay);

        ApplyDamageToPlayer(WideSweepingStrikes1, wideSweepingStrikesDamage);

        WideSweepingStrikes1.SetActive(false);

        // Second strike
        yield return new WaitForSeconds(wideSweepingStrikes2Delay); // Unique delay for Wide Sweeping Strikes 2

        WideSweepingStrikes2.SetActive(true);
        TriggerAnimation("WideSweepingStrikes2");

        yield return new WaitForSeconds(damageDelay);

        ApplyDamageToPlayer(WideSweepingStrikes2, wideSweepingStrikesDamage);

        WideSweepingStrikes2.SetActive(false);
    }

    private IEnumerator OverheadSlams()
    {
        yield return new WaitForSeconds(overheadSlamsDelay); // Unique delay for Overhead Slams

        OverheadSlamsHitbox.SetActive(true);
        TriggerAnimation("OverheadSlams");

        yield return new WaitForSeconds(damageDelay);

        ApplyDamageToPlayer(OverheadSlamsHitbox, overheadSlamsDamage);

        yield return new WaitForSeconds(hitboxDuration);

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

    private void TriggerAnimation(string animationName)
    {
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger(animationName);
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
