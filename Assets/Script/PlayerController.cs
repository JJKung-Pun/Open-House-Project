using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float dashSpeed = 20f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 5f;
    public float dashCooldown = 1f;

    private float dashTime = 0f;
    private bool isDashing = false;
    private Rigidbody2D rb;
    private Vector2 movement;

    public Slider staminaBar;

    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateStaminaBar();
    }

    void Update()
    {
        
        Move();
        Sprint();
        Dash();
        UpdateStaminaBar(); 
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector2(moveHorizontal, moveVertical);
        
        if (movement != Vector2.zero)
        {
            float speed = isDashing ? dashSpeed : (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed);
            rb.velocity = movement.normalized * speed;

            if ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= staminaDrainRate * Time.deltaTime;
                if (stamina < 0) stamina = 0;
            }
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashTime <= 0)
        {
            if (stamina >= 30f) 
            {
                isDashing = true;
                stamina -= 30f;
                rb.velocity = rb.velocity.normalized * dashSpeed;
                dashTime = dashCooldown;
            }
        }
        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.value = stamina; 
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
