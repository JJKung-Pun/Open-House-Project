using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    public GameObject WideSweepingStrikes1; // First hitbox
    public GameObject WideSweepingStrikes2; // Second hitbox
    public GameObject OverheadSlamsHitbox; // Hitbox for Overhead Slams
    public GameObject ChainSweepsHitbox; // Hitbox for Chain Sweeps
    public int firstHitDamage = 70; // Damage for first hitbox
    public int secondHitDamage = 30; // Damage for second hitbox
    public int overheadSlamsDamage = 80; // Damage for Overhead Slams
    public int chainSweepsDamage = 20; // Damage for Chain Sweeps
    public float hitbox1Duration = 2.0f; // Time for first hitbox to stay visible
    public float hitbox2Duration = 1.0f; // Time for second hitbox to stay visible
    public float overheadSlamsDuration = 3.0f; // Duration for Overhead Slams hitbox
    public float chainSweepsDuration = 2.0f; // Duration for Chain Sweeps hitbox
    public float rangeToPlayer = 5.0f; // Range within which the boss will attack

    private bool isPlayerInRange = false; // Track if player is in range
    private bool isAttacking = false; // Track if the boss is currently attacking
    private SpriteRenderer hitbox1Renderer; // Reference to first hitbox renderer
    private SpriteRenderer hitbox2Renderer; // Reference to second hitbox renderer
    private SpriteRenderer overheadSlamsRenderer; // Reference to Overhead Slams hitbox renderer
    private SpriteRenderer chainSweepsRenderer; // Reference to Chain Sweeps hitbox renderer

    private void Start()
    {
        hitbox1Renderer = WideSweepingStrikes1.GetComponent<SpriteRenderer>();
        hitbox2Renderer = WideSweepingStrikes2.GetComponent<SpriteRenderer>();
        overheadSlamsRenderer = OverheadSlamsHitbox.GetComponent<SpriteRenderer>();
        chainSweepsRenderer = ChainSweepsHitbox.GetComponent<SpriteRenderer>();

        hitbox1Renderer.enabled = false;
        hitbox2Renderer.enabled = false;
        overheadSlamsRenderer.enabled = false;
        chainSweepsRenderer.enabled = false;
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            isPlayerInRange = distanceToPlayer <= rangeToPlayer;

            // Check if the boss can attack
            if (isPlayerInRange && !isAttacking)
            {
                StartCoroutine(SelectAndPerformRandomAttack());
            }
        }
    }

    private IEnumerator SelectAndPerformRandomAttack()
    {
        isAttacking = true;

        // Randomly select one of the three available attacks
        List<string> availableAttacks = new List<string> { "OverheadSlams", "ChainSweeps", "WideSweepingStrikes" };
        int randomIndex = Random.Range(0, availableAttacks.Count);
        string selectedAttack = availableAttacks[randomIndex];

        // Execute the selected attack
        if (selectedAttack == "OverheadSlams")
        {
            yield return OverheadSlams();
        }
        else if (selectedAttack == "ChainSweeps")
        {
            yield return ChainSweeps();
        }
        else if (selectedAttack == "WideSweepingStrikes")
        {
            yield return WideSweepingStrikes();
        }

        // After performing the attack, wait for a cooldown period before selecting a new attack
        float cooldownDuration = Random.Range(3f, 5f);
        yield return new WaitForSeconds(cooldownDuration);

        isAttacking = false;
    }

    private IEnumerator ChainSweeps()
    {
        // Disable movement
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false; // Ensure the enemy doesn't move
        }

        // Chain Sweeps hitbox active
        chainSweepsRenderer.enabled = true;
        yield return new WaitForSeconds(chainSweepsDuration);

        // Damage player if in chain sweeps range (allows parry)
        DealDamageToPlayer(chainSweepsDamage, ChainSweepsHitbox);
        chainSweepsRenderer.enabled = false;

        // Re-enable movement immediately after the attack
        if (enemyMovement != null)
        {
            enemyMovement.enabled = true; // Allow movement again
        }
    }

    private IEnumerator WideSweepingStrikes()
    {
        // Disable movement
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false; // Ensure the enemy doesn't move
        }

        // First hitbox active
        hitbox1Renderer.enabled = true;
        yield return new WaitForSeconds(hitbox1Duration);
        hitbox1Renderer.enabled = false;

        // Damage player if in first hitbox range (ignores parry)
        DealDamageToPlayer(firstHitDamage, WideSweepingStrikes1);

        // Second hitbox active
        hitbox2Renderer.enabled = true;
        yield return new WaitForSeconds(hitbox2Duration);
        hitbox2Renderer.enabled = false;

        // Damage player if in second hitbox range (ignores parry)
        DealDamageToPlayer(secondHitDamage, WideSweepingStrikes2);

        // Re-enable movement immediately after the attack
        if (enemyMovement != null)
        {
            enemyMovement.enabled = true; // Allow movement again
        }
    }

    private IEnumerator OverheadSlams()
    {
        // Disable movement
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false; // Ensure the enemy doesn't move
        }

        // Overhead Slams hitbox active
        overheadSlamsRenderer.enabled = true;
        yield return new WaitForSeconds(overheadSlamsDuration);

        // Damage player if in overhead slams range (ignores parry)
        DealDamageToPlayer(overheadSlamsDamage, OverheadSlamsHitbox);
        overheadSlamsRenderer.enabled = false;

        // Re-enable movement immediately after the attack
        if (enemyMovement != null)
        {
            enemyMovement.enabled = true; // Allow movement again
        }
    }

    private void DealDamageToPlayer(int damage, GameObject hitbox)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (hitbox.GetComponent<Collider2D>().IsTouching(playerCollider))
            {
                // If Chain Sweeps, allow parry/block
                if (hitbox == ChainSweepsHitbox)
                {
                    // Allow parry/block logic
                    Debug.Log("Player can parry/block Chain Sweeps.");
                }
                else
                {
                    // Ignore parry, directly apply damage
                    player.GetComponent<PlayerHP>().TakeDamage(damage);
                    Debug.Log("Player took " + damage + " damage.");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}