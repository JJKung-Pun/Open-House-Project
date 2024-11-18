using System.Collections;
using UnityEngine;
using TMPro;

public class Boss1QTE : MonoBehaviour
{
    public TMP_Text qteText;
    public GameObject qtePopup;
    public GameObject ePopupText;
    public GameObject popupCanvas;

    private bool isInQTE = false;  // This will track whether QTE is active
    private KeyCode qteKey;
    private float qteDuration = 6f;
    public float activationRange = 2f;
    private int failCount = 0;
    private bool isPlayerInRange = false;

    // Property to access the current status of the QTE
    public bool IsInQTE
    {
        get { return isInQTE; }
    }

    void Update()
    {
        // Only show the 'E' prompt when the player is in range
        if (isPlayerInRange)
        {
            popupCanvas.SetActive(true);  // Show PopupCanvas
            ePopupText.SetActive(true);   // Show 'E' prompt when in range

            // Check if the player presses 'E' to start the QTE
            if (Input.GetKeyDown(KeyCode.E) && !isInQTE)
            {
                Boss1HP bossHP = GetComponent<Boss1HP>();
                if (bossHP != null && bossHP.currentHealth == 1) // Only start QTE if HP is 1
                {
                    StartQTE();
                    ePopupText.SetActive(false);  // Hide 'E' prompt when starting QTE
                }
            }
        }
        else
        {
            ePopupText.SetActive(false);  // Hide 'E' prompt if not in range
            popupCanvas.SetActive(false); // Optionally deactivate PopupCanvas if not in range
        }

        // Handle the QTE if it's in progress
        if (isInQTE)
        {
            // Check if the player presses the correct key for QTE
            if (Input.GetKeyDown(qteKey))
            {
                EndQTE(true);  // Success
            }
            else if (failCount < 1 && !Input.GetKeyDown(qteKey))
            {
                failCount++;
                EndQTE(false); // Fail if wrong key pressed
            }
        }
    }

    private bool CanStartQTE()
    {
        Boss1HP bossHP = GetComponent<Boss1HP>();
        return bossHP != null && bossHP.CanStartQTE && isPlayerInRange; // Fixed property usage
    }

    public void StartQTE()
    {
        if (CanStartQTE())
        {
            isInQTE = true;
            qteKey = GetRandomKey();
            DisplayQTEPrompt(qteKey);
            failCount = 0;
            StartCoroutine(QTECoroutine());
        }
    }

    private IEnumerator QTECoroutine()
    {
        yield return new WaitForSeconds(qteDuration);
        EndQTE(false, true);  // Timed out
    }

    public void EndQTE(bool success, bool timedOut = false)
    {
        isInQTE = false;
        HideQTEPrompt();

        Boss1HP bossHP = GetComponent<Boss1HP>();
        if (bossHP != null)
        {
            if (success)
            {
                bossHP.currentHealth = 0;
                bossHP.UpdateHealthBar();
            }
            else if (timedOut || failCount >= 1)
            {
                bossHP.ResetHealth();
            }
        }
    }

    private KeyCode GetRandomKey()
    {
        int randomValue = Random.Range(0, 26);
        return (KeyCode)(randomValue + (int)KeyCode.A);
    }

    private void DisplayQTEPrompt(KeyCode key)
    {
        if (qteText != null && qtePopup != null)
        {
            qteText.text = key.ToString();  // Show the correct key for the QTE
            qtePopup.SetActive(true);
        }
    }

    private void HideQTEPrompt()
    {
        if (qtePopup != null)
        {
            qtePopup.SetActive(false);
        }
    }

    // Called to track player entering and leaving range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
