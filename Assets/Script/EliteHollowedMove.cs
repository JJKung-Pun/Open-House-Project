using UnityEngine;

public class EliteHollowedMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopRange = 0.5f;
    [SerializeField] private float detectionRange = 4f;

    private Transform player;
    private bool isMoving = false;
    private Transform hitbox;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hitbox = transform.Find("Hitbox");
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // If within detection range and not too close to the player
        if (distance <= detectionRange && distance > stopRange)
        {
            isMoving = true;
            MoveTowardsPlayer();
        }
        else
        {
            isMoving = false;
            // Play idle animation when not moving
            animator.SetBool("IsMoving", false); // Idle state
        }

        // Update animation states based on movement
        if (isMoving)
        {
            animator.SetBool("IsMoving", true); // Walking state
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Set rotation based on movement direction
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); // Facing right
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Facing left
        }

        // Move the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Rotate the hitbox to face the direction of movement
        if (hitbox != null)
        {
            hitbox.right = direction;
        }
    }
}
