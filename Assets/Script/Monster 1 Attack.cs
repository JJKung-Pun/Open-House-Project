using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Attack : MonoBehaviour
{
    public GameObject hitbox; 
    public float hitboxVisibleDuration = 3.0f; 
    public int attackDamage = 35; 
    public int poisonDamage = 1; 
    public float poisonDuration = 15.0f; 
    public float rangeToPlayer = 3.0f; 
    public float attackCooldown = 5.0f; 

    private bool isPlayerInRange = false; 
    public bool isAttacking = false; 

    private SpriteRenderer hitboxRenderer; 
    private MovementMonster1 movementScript; 
    private Collider2D hitboxCollider; 

    private void Start()
    {
        hitboxRenderer = hitbox.GetComponent<SpriteRenderer>();
        movementScript = GetComponent<MovementMonster1>(); 
        hitboxCollider = hitbox.GetComponent<Collider2D>();

        hitboxRenderer.enabled = false; 
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            isPlayerInRange = distanceToPlayer <= rangeToPlayer;

            if (isPlayerInRange && !isAttacking)
            {
                StartCoroutine(HandleAttack(player));
            }
        }
    }

    private IEnumerator HandleAttack(GameObject player)
    {
        isAttacking = true;
        movementScript.enabled = false; 

        hitboxRenderer.enabled = true; 
        yield return new WaitForSeconds(hitboxVisibleDuration); 

        if (hitboxCollider.IsTouching(player.GetComponent<Collider2D>()))
        {
            PlayerHP playerHP = player.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(attackDamage); 
                StartCoroutine(ApplyPoison(playerHP)); 
            }
        }

        hitboxRenderer.enabled = false; 
        yield return new WaitForSeconds(attackCooldown);
        movementScript.enabled = true; 
        isAttacking = false; 
    }

    private IEnumerator ApplyPoison(PlayerHP playerHP)
    {
        float poisonTime = 0;
        while (poisonTime < poisonDuration)
        {
            playerHP.TakeDamage(poisonDamage); 
            poisonTime += 1.0f;
            yield return new WaitForSeconds(1.0f); 
        }
    }
}
