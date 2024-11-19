using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
{
    private GameObject lightAttackArea;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private PlayerController playerController;

    [SerializeField] private int attackDamage = 3;

    void Start()
    {
        lightAttackArea = transform.GetChild(0).gameObject;
        lightAttackArea.SetActive(false);
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                lightAttackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        lightAttackArea.SetActive(attacking);
        playerController.DisableMovementForDuration(1f);
        playerController.StopMovement();

        // Check for hits on enemies
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(lightAttackArea.transform.position, lightAttackArea.transform.localScale, 0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) // Make sure the enemy has the "Enemy" tag
            {
                EliteHollowedHealth health = hitCollider.GetComponent<EliteHollowedHealth>();
                if (health != null)
                {
                    Debug.Log("Enemy hit! Applying damage.");
                    health.TakeDamage(attackDamage); // Apply damage to the enemy
                }
            }
        }
    }
}
