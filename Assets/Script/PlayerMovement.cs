using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float sprintSpeed = 10f; 
    public float dashSpeed = 20f; 
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f;

    private Rigidbody2D rb; 
    private Vector2 movement; 

    private bool isDashing = false; 
    private float dashTime = 0f;
    private float lastDashTime = -1f; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

     
        movement = new Vector2(moveX, moveY).normalized;

    
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = movement * dashSpeed;
            dashTime -= Time.fixedDeltaTime;

            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            rb.velocity = movement * currentSpeed;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        lastDashTime = Time.time;
    }

    private void EndDash()
    {
        isDashing = false;
    }
}
