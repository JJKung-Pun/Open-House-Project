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

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(lightAttackArea.transform.position, lightAttackArea.transform.localScale, 0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EliteHollowedHealth eliteHealth = hitCollider.GetComponent<EliteHollowedHealth>();
                if (eliteHealth != null)
                {
                    eliteHealth.TakeDamage(attackDamage);
                    continue; // Move to the next enemy after processing this one
                }

                HollowedSoldierHealth soldierHealth = hitCollider.GetComponent<HollowedSoldierHealth>();
                if (soldierHealth != null)
                {
                    soldierHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
}
