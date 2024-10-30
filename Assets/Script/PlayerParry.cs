using System.Collections;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    private bool isParrying;
    private bool isBlocking;
    public float parryTime = 1.0f;
    public float staminaDrain = 20f;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerController.stamina >= staminaDrain)
        {
            isParrying = true;
            StartCoroutine(ParryCoroutine());
        }
        else if (Input.GetKey(KeyCode.F) && playerController.stamina > 0)
        {
            isBlocking = true;
            playerController.DrainStamina(staminaDrain * Time.deltaTime);
        }
        else
        {
            isBlocking = false;
        }
    }

    private IEnumerator ParryCoroutine()
    {
        playerController.DrainStamina(staminaDrain);
        yield return new WaitForSeconds(parryTime);
        isParrying = false;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}
