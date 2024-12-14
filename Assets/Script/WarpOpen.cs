using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D warpCollider; // Assign the warp collider in the inspector
    private bool isWarpActive = false;

    void Start()
    {
        // Ensure the warp collider is initially disabled
        if (warpCollider != null)
        {
            warpCollider.enabled = false;
        }
    }
    void Update()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        // Find all objects with the tag "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If no enemies remain, activate the warp collider
        if (enemies.Length == 0 && warpCollider != null && !isWarpActive)
        {
            warpCollider.enabled = true; // Enable the BoxCollider2D
            isWarpActive = true;
            Debug.Log("Warp collider enabled!"); // Debug message for verification
        }
    }
}
