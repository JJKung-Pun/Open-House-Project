using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
{
    private GameObject lightAttackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private PlayerController playerController;

    // Damage value
    public int attackDamage = 3;

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

        // Check for hits on the boss
        Collider[] hitColliders = Physics.OverlapSphere(lightAttackArea.transform.position, lightAttackArea.transform.localScale.x / 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Boss")) // Ensure your boss has the "Boss" tag
            {
                hitCollider.GetComponent<Boss1HP>().Damage(attackDamage); // Apply damage to the boss
            }
        }
    }
}
