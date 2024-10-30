using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f; 
    public float stopDistance = 0.5f;

    private float distance;
    private float fixedY;
    private bool canMove = true; // Controls whether the enemy can move

    void Start()
    {
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (player != null && canMove)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > stopDistance)
            {
                Vector2 targetPosition = new Vector2(player.transform.position.x, fixedY);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void StartMovement()
    {
        canMove = true;
    }
}
