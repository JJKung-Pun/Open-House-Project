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

    void Start()
    {
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > stopDistance)
            {
                Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;

                Vector2 targetPosition = new Vector2(player.transform.position.x, fixedY);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }
}
