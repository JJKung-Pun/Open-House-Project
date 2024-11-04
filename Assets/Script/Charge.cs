using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1DamageArea : MonoBehaviour
{
    public GameObject hitBlockPrefab; // Reference to the hit block prefab
    public float initialDelay = 3f; // Delay before showing the hit block

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerAttack")) // Assuming player attack has this tag
        {
            StartCoroutine(ShowHitBlockAfterDelay(transform.position)); // Start the coroutine to show the hit block
        }
    }

    private IEnumerator ShowHitBlockAfterDelay(Vector2 position)
    {
        // Wait for 3 seconds before showing the hit block
        yield return new WaitForSeconds(initialDelay);

        // Instantiate the hit block at the boss's position
        GameObject hitBlock = Instantiate(hitBlockPrefab, position, Quaternion.identity);
        Animator hitBlockAnimator = hitBlock.GetComponent<Animator>();

        // Trigger the hit animation
        if (hitBlockAnimator != null)
        {
            hitBlockAnimator.SetTrigger("Hit"); // Trigger the hit animation
        }

        // Optionally destroy the hit block after a delay
        Destroy(hitBlock, 0.5f); // Adjust the duration as needed
    }
}
