using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public PlayerHP playerHP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerHP.TakeDamage(damageAmount);  // Example damage amount
        }
    }
}