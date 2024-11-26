using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float Speed = 4.5f;
    private float isRight;
    public Boss1Movement Boss;

    private void Start()
    {
        if(Boss.isTurnRight)
        {
            isRight = 1f;
        }
        else
        {
            isRight = -1f;
        }
    }
    void Update()
    {
        transform.position += (isRight * transform.right) * Time.deltaTime * Speed;
    }
}
