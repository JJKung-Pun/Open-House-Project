using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackArea : MonoBehaviour
{
    private int damage = 3;
    public GameObject hitBlockPrefab; // Reference to the hit block prefab

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Boss1HP health = collider.GetComponent<Boss1HP>();
        if (health != null)
        {
            health.Damage(damage);
            ShowHitBlock(collider.transform.position); // Show the hit block at the boss's position
        }
    }

    private void ShowHitBlock(Vector2 position)
    {
        GameObject hitBlock = Instantiate(hitBlockPrefab, position, Quaternion.identity);
        Animator hitBlockAnimator = hitBlock.GetComponent<Animator>();
        if (hitBlockAnimator != null)
        {
            hitBlockAnimator.SetTrigger("Hit"); // Trigger the hit animation
        }

        // Optionally destroy the hit block after a delay
        Destroy(hitBlock, 0.5f); // Adjust the duration as needed
    }
}
