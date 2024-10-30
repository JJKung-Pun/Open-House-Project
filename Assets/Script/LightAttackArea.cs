using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyHealth health = collider.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.Damage(damage); 
        }
    }
}
