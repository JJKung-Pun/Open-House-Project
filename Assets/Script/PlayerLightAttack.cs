using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttack : MonoBehaviour
{
    private GameObject lightAttackArea = default;
    private bool attacking = false;
    private float timetoAttack = 0.25f;
    private float timer = 0f;

    void Start()
    {
        lightAttackArea = transform.GetChild(0).gameObject;     
        lightAttackArea.SetActive(false); 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

        if(attacking)
        {
            timer += Time.deltaTime;

            if(timer >= timetoAttack)
            {
                timer = 0;
                attacking = false;
                lightAttackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        lightAttackArea.SetActive(attacking);
    }
}
