using UnityEngine;
using TMPro;

public class BossPopup : MonoBehaviour
{
    public GameObject popupCanvas;  // The canvas showing the popup
    public TMP_Text QTEText;        // TMP_Text component for the QTE text
    public TMP_Text EPopupText;     // Alternate TMP_Text component (for fallback)
    private bool playerInRange = false;  // Whether the player is in range of the boss
    private bool isQTEActive = false;    // Whether QTE is currently active
    private Boss1QTE bossQTE;            // Reference to Boss1QTE script
    private Boss1HP bossHP;              // Reference to Boss1HP script

    void Start()
    {
        popupCanvas.SetActive(false);  // Start with the popup hidden
        bossQTE = GetComponent<Boss1QTE>();
        bossHP = GetComponent<Boss1HP>();

        if (QTEText == null && EPopupText == null)
        {
            Debug.LogError("Neither QTEText nor EPopupText is assigned in the BossPopup script.");
        }
    }

    void Update()
    {
        // Use the CanStartQTE property correctly (without parentheses)
        if (playerInRange && bossHP != null && bossHP.CanStartQTE && !isQTEActive)
        {
            popupCanvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                isQTEActive = true;
                bossQTE.StartQTE();
                popupCanvas.SetActive(false);
            }
        }
        else
        {
            popupCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bossHP != null && bossHP.currentHealth == 1 && !isQTEActive)
        {
            playerInRange = true;
            Debug.Log("Player entered range. Displaying random QTE key.");
            DisplayRandomQTEKey();  // Show random key for the QTE
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popupCanvas.SetActive(false);  // Hide the popup when the player exits range
            Debug.Log("Player exited range. Hiding popup.");
        }
    }

    // Display a random key (A-Z) for the QTE prompt
    private void DisplayRandomQTEKey()
    {
        if (QTEText != null || EPopupText != null)
        {
            TMP_Text selectedText = QTEText != null ? QTEText : EPopupText;

            // Generate random key and display it
            KeyCode randomKey = (KeyCode)Random.Range((int)KeyCode.A, (int)KeyCode.Z + 1);
            selectedText.text = randomKey.ToString();  // Set the text to the random key
            Debug.Log("Random QTE key displayed: " + randomKey.ToString());
        }
        else
        {
            Debug.LogError("Neither QTEText nor EPopupText is assigned in the BossPopup script.");
        }
    }
}
