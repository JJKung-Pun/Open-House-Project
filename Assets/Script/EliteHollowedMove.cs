using UnityEngine;

public class EliteHollowedMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopRange = 0.5f;
    [SerializeField] private float detectionRange = 4f;

    private Transform player;
    private bool isMoving = false;
    private Transform hitbox;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hitbox = transform.Find("Hitbox");
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            isMoving = true;
        }

        if (isMoving && distance > stopRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            isMoving = false;
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
            hitbox.up = direction;
        }
    }
}
