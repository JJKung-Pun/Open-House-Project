using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Boss1HP health = collider.GetComponent<Boss1HP>();
        if (health != null)
        {
            health.Damage(damage);
        }
    }
}
