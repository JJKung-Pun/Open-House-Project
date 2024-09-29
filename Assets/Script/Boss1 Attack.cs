using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack : MonoBehaviour
{
    public GameObject hitbox;
    public float attackCooldown = 1.0f;
    public int attackDamage = 30;
    public float hitboxVisibilityDuration = 4.0f;

    private bool isPlayerInHitbox = false;
    private bool hasAttacked = false;
    private SpriteRenderer hitboxRenderer;

    private PlayerParry playerParry;

    private void Start()
    {
        hitboxRenderer = hitbox.GetComponent<SpriteRenderer>();
        hitboxRenderer.enabled = false;
    }

    void Update()
    {
        if (isPlayerInHitbox && !hasAttacked)
        {
            StartCoroutine(ShowHitbox());
        }
    }

    private void Attack()
    {
        Debug.Log("Boss1 is attacking!");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerParry = player.GetComponent<PlayerParry>();

            if (playerParry.isBlocking)
            {
                int damageToDeal = Mathf.FloorToInt(attackDamage * 0.8f);
                player.GetComponent<PlayerHP>().TakeDamage(damageToDeal);
                Debug.Log("Player is blocking! Damage reduced to: " + damageToDeal);
            }
            else if (playerParry.isParrying)
            {
                playerParry.OnEnemyAttack(gameObject, attackDamage);
            }
            else
            {
                player.GetComponent<PlayerHP>().TakeDamage(attackDamage);
            }
        }

        hasAttacked = true;
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        hasAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInHitbox = true;
            hasAttacked = false;
            StartCoroutine(ShowHitbox());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInHitbox = false;
            hitboxRenderer.enabled = false;
            hasAttacked = false;
        }
    }

    private IEnumerator ShowHitbox()
    {
        hitboxRenderer.enabled = true;
        yield return new WaitForSeconds(hitboxVisibilityDuration);
        if (isPlayerInHitbox && !hasAttacked)
        {
            Attack();
        }
        hitboxRenderer.enabled = false;
    }
}
