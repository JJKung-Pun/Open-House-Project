using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float Speed = 4.5f;
    public Boss1Movement Boss;  // Reference to Boss's movement script
    private Vector3 direction;  // Fireball's movement direction

    private void Start()
    {
        // Initially, set the direction based on Boss's facing direction
        if (Boss.isTurnRight)
        {
            direction = Vector3.right;  // Fireball moves right if the boss is facing right
        }
        else
        {
            direction = Vector3.left;   // Fireball moves left if the boss is facing left
        }
    }

    void Update()
    {
        // Update the direction based on the boss's current facing
        if (Boss.isTurnRight)
        {
            direction = Vector3.right;  // Fireball goes right
        }
        else
        {
            direction = Vector3.left;   // Fireball goes left
        }

        // Move the fireball in the determined direction
        transform.position += direction * Speed * Time.deltaTime;
    }
}
