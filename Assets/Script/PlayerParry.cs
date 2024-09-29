using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    public float parryWindow = 1f; 
    public bool isParrying = false; 
    public bool isBlocking = false; 
    public GameObject parryEffect; 
    public GameObject blockEffect; 
    public float knockbackForce = 10f; 
    public float knockbackDuration = 0.5f; 
    public float knockbackCooldown = 1f; 

    private PlayerController playerController; 
    public int parryStaminaCost = 20; 
    public float blockStaminaDrainRate = 5f; 

    private Coroutine parryCoroutine;
    private Coroutine blockCoroutine;
    private bool enemyKnockedBack = false;
    private Coroutine knockbackCoroutine; 

    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isBlocking && playerController.stamina >= parryStaminaCost) 
            {
                parryCoroutine = StartCoroutine(Parry());
            }
        }
        else if (Input.GetKey(KeyCode.F))
        {
            if (!isBlocking && !isParrying && playerController.stamina > 0) 
            {
                StartBlock();
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            StopBlock();
        }
    }

    IEnumerator Parry()
    {
        playerController.stamina -= parryStaminaCost;
        isParrying = true;

        if (parryEffect != null)
        {
            Instantiate(parryEffect, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(parryWindow);

        isParrying = false;

        if (Input.GetKey(KeyCode.F))
        {
            StartBlock();
        }

        parryCoroutine = null; 
    }

    void StartBlock()
    {
        isBlocking = true;

        if (blockEffect != null)
        {
            Instantiate(blockEffect, transform.position, Quaternion.identity);
        }

        if (blockCoroutine == null)
        {
            blockCoroutine = StartCoroutine(Block());
        }
    }

    void StopBlock()
    {
        isBlocking = false;

        if (blockCoroutine != null)
        {
            StopCoroutine(blockCoroutine);
            blockCoroutine = null;
        }
    }

    IEnumerator Block()
    {
        while (isBlocking)
        {
            if (playerController.stamina > 0) 
            {
                playerController.stamina -= Mathf.RoundToInt(blockStaminaDrainRate * Time.deltaTime); 
                playerController.stamina = Mathf.Max(playerController.stamina, 0); 
            }
            else
            {
                StopBlock(); 
            }
            yield return null; 
        }
    }

    public void OnEnemyAttack(GameObject enemy, int damage)
    {
        if (isParrying)
        {
            KnockbackEnemy(enemy); 
            return; 
        }
        else if (isBlocking)
        {
            damage = Mathf.RoundToInt(damage * 0.8f); 
        }

        PlayerHP playerHP = GetComponent<PlayerHP>();
        playerHP?.TakeDamage(damage);
    }

    void KnockbackEnemy(GameObject enemy)
    {
        if (enemyKnockedBack) return; 

        enemyKnockedBack = true; 
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

        if (enemyRb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            if (knockbackCoroutine != null)
            {
                StopCoroutine(knockbackCoroutine); 
            }
            knockbackCoroutine = StartCoroutine(HandleKnockbackCooldown(enemy));
        }
    }

    IEnumerator HandleKnockbackCooldown(GameObject enemy)
    {
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.enabled = false; 
        }

        yield return new WaitForSeconds(knockbackDuration); 

        if (enemyMovement != null)
        {
            enemyMovement.enabled = true; 
        }

        yield return new WaitForSeconds(knockbackCooldown); 

        enemyKnockedBack = false;
    }
}