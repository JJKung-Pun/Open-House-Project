using System.Collections;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    // Hitbox game objects for different attacks
    public GameObject WideSweepingStrikes1;
    public GameObject WideSweepingStrikes2;
    public GameObject OverheadSlamsHitbox;
    public GameObject ChainSweepsHitbox;

    // Damage values for each attack type
    public int wideSweepingStrikesDamage = 35;
    public int overheadSlamsDamage = 45;
    public int chainSweepsDamage = 30;

    // Attack properties
    public float hitboxDuration = 1.0f; // Duration for which each hitbox is active
    public float attackCooldown = 5.0f; // Time before the next attack can occur
    public float rangeToPlayer = 5.0f; // The range within which the boss can attack
    public float hitboxDisplayTime = 3.0f; // Time to display hitbox before dealing damage

    private bool isPlayerInRange = false; // Indicates if the player is within attack range
    private bool isAttacking = false; // Indicates if the boss is currently attacking
    private EnemyMovement enemyMovement; // Reference to the enemy movement script

    private void Start()
    {
        // Get the enemy movement component
        enemyMovement = GetComponent<EnemyMovement>();
        // Disable all hitboxes initially
        DisableAllHitboxes();
    }

    private void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Calculate distance to player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            isPlayerInRange = distanceToPlayer <= rangeToPlayer;

            // If the player is in range and the boss is not currently attacking
            if (isPlayerInRange && !isAttacking)
            {
                StartCoroutine(SelectRandomAttack());
            }
        }
    }

    private IEnumerator SelectRandomAttack()
    {
        isAttacking = true; // Mark the boss as attacking

        // Stop movement at the start of the attack
        if (enemyMovement != null)
        {
            enemyMovement.StopMovement();
        }

        // Select a random attack type
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

        // Wait for the cooldown duration before allowing another attack
        yield return new WaitForSeconds(attackCooldown);

        // Resume movement after the attack is finished
        if (enemyMovement != null)
        {
            enemyMovement.StartMovement();
        }

        isAttacking = false; // Mark the boss as not attacking anymore
    }

    private IEnumerator ChainSweeps()
    {
        ChainSweepsHitbox.SetActive(true); // Activate the chain sweeps hitbox
        yield return new WaitForSeconds(hitboxDisplayTime); // Wait for hitbox display time
        yield return new WaitForSeconds(hitboxDuration); // Wait for the duration

        // Check for damage after the hitbox duration
        CheckForDamage(ChainSweepsHitbox, chainSweepsDamage);
        ChainSweepsHitbox.SetActive(false); // Deactivate the hitbox
    }

    private IEnumerator WideSweepingStrikes()
    {
        WideSweepingStrikes1.SetActive(true); // Activate the first hitbox
        yield return new WaitForSeconds(hitboxDisplayTime); // Wait for hitbox display time
        yield return new WaitForSeconds(hitboxDuration); // Wait for the duration
        
        // Check for damage after the hitbox duration
        CheckForDamage(WideSweepingStrikes1, wideSweepingStrikesDamage);
        WideSweepingStrikes1.SetActive(false); // Deactivate the first hitbox

        WideSweepingStrikes2.SetActive(true); // Activate the second hitbox
        yield return new WaitForSeconds(hitboxDisplayTime); // Wait for hitbox display time
        yield return new WaitForSeconds(hitboxDuration); // Wait for the duration
        
        // Check for damage after the hitbox duration
        CheckForDamage(WideSweepingStrikes2, wideSweepingStrikesDamage);
        WideSweepingStrikes2.SetActive(false); // Deactivate the second hitbox
    }

    private IEnumerator OverheadSlams()
    {
        OverheadSlamsHitbox.SetActive(true); // Activate the overhead slams hitbox
        yield return new WaitForSeconds(hitboxDisplayTime); // Wait for hitbox display time
        yield return new WaitForSeconds(hitboxDuration); // Wait for the duration
        
        // Check for damage after the hitbox duration
        CheckForDamage(OverheadSlamsHitbox, overheadSlamsDamage);
        OverheadSlamsHitbox.SetActive(false); // Deactivate the hitbox
    }

    private void CheckForDamage(GameObject hitbox, int damage)
    {
        // Get the collider size for the hitbox
        BoxCollider2D boxCollider = hitbox.GetComponent<BoxCollider2D>();
        Vector2 size = boxCollider.size;

        // Check for players in the hitbox area
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(hitbox.transform.position, size, 0);
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
        // Ensure all hitboxes are deactivated at the start
        WideSweepingStrikes1.SetActive(false);
        WideSweepingStrikes2.SetActive(false);
        OverheadSlamsHitbox.SetActive(false);
        ChainSweepsHitbox.SetActive(false);
    }

    public void StopTargetingPlayer()
    {
        // Stop targeting the player if HP reaches zero
        isPlayerInRange = false;
        isAttacking = false;
        if (enemyMovement != null)
        {
            enemyMovement.StopMovement();
        }
    }
}
