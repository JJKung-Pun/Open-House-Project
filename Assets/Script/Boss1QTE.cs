using System.Collections;
using UnityEngine;
using TMPro;

public class Boss1QTE : MonoBehaviour
{
    public TMP_Text qteText;
    public GameObject qtePopup;
    public GameObject ePopupText;
    public GameObject popupCanvas;

    private bool isInQTE = false;
    private KeyCode qteKey;
    private float qteDuration = 6f;
    public float activationRange = 2f;
    private int failCount = 0;
    private bool isPlayerInRange = false;

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
                    Debug.Log("QTE started.");
                    StartQTE();
                    ePopupText.SetActive(false);  // Hide 'E' prompt when starting QTE
                }
                else
                {
                    Debug.Log("Boss HP is not 1, QTE cannot start.");
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
                Debug.Log("QTE success.");
                EndQTE(true);  // Success
            }
            else if (failCount < 1 && !Input.GetKeyDown(qteKey))
            {
                failCount++;
                Debug.Log("QTE failed attempt.");
                EndQTE(false); // Fail if wrong key pressed
            }
        }
    }

    private bool CanStartQTE()
    {
        Boss1HP bossHP = GetComponent<Boss1HP>();
        if (bossHP != null && bossHP.currentHealth == 1) // Ensure health is 1 before QTE can start
        {
            // Ensure the player is within the activation range
            float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            return distance <= activationRange && isPlayerInRange;  // Check both conditions
        }
        return false;
    }

    public void StartQTE()
    {
        // Only start the QTE if the boss HP is 1 and the player is in range
        if (CanStartQTE())
        {
            isInQTE = true;
            qteKey = GetRandomKey();
            DisplayQTEPrompt(qteKey);
            failCount = 0; // Reset fail count for new QTE
            StartCoroutine(QTECoroutine());
        }
        else
        {
            Debug.Log("Cannot start QTE. Conditions not met.");
        }
    }

    private IEnumerator QTECoroutine()
    {
        yield return new WaitForSeconds(qteDuration);
        Debug.Log("QTE timed out.");
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
            qteText.text = key.ToString();
            qtePopup.SetActive(true);
            Debug.Log($"QTE Prompt Displayed: {key.ToString()}");
        }
    }

    private void HideQTEPrompt()
    {
        if (qtePopup != null)
        {
            qtePopup.SetActive(false);
            Debug.Log("QTE Prompt Hidden.");
        }
    }

    public bool IsInQTE()
    {
        return isInQTE;
    }

    // Methods to track player entering and exiting range
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            popupCanvas.SetActive(true);  // Show the PopupCanvas when the player enters range
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            popupCanvas.SetActive(false); // Hide the PopupCanvas when the player exits the range
        }
    }
}
