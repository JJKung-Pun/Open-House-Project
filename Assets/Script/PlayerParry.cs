using System.Collections;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    private bool isParrying;
    private bool isBlocking;

    public float parryTime = 1.0f; // Duration for a parry to remain active
    public float staminaDrain = 20f; // Stamina consumed per parry or per second of blocking

    private PlayerController playerController;
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>(); // Assign Animator component
    }

    void Update()
    {
        // Parry input
        if (Input.GetKeyDown(KeyCode.K) && playerController.stamina >= staminaDrain)
        {
            isParrying = true;
            StartCoroutine(ParryCoroutine());
        }
        // Blocking input
        else if (Input.GetKey(KeyCode.K) && !isParrying && playerController.stamina > 0)
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
        // Trigger the start animation
        animator.SetTrigger("ParryStart");

        playerController.DrainStamina(staminaDrain);

        // Wait for the start animation to finish (adjust duration if needed)
        yield return new WaitForSeconds(0.2f); // Adjust based on start animation duration

        // Trigger the parry loop animation
        animator.SetTrigger("ParryLoop");

        // Parry remains active for the duration of parryTime
        yield return new WaitForSeconds(parryTime - 0.2f); // Subtract start animation duration

        // Parry ends, trigger the end animation
        animator.SetTrigger("ParryEnd");

        yield return new WaitForSeconds(0.2f); // Adjust based on end animation duration

        isParrying = false;
    }

    // Check if the player is currently parrying
    public bool IsParrying()
    {
        return isParrying;
    }

    // Check if the player is currently blocking
    public bool IsBlocking()
    {
        return isBlocking;
    }
}
