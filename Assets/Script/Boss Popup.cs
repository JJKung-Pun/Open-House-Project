using UnityEngine;
using TMPro;  // Import TMP namespace

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
        // Check if the player is in range and boss's health is 1
        if (playerInRange && bossHP != null && bossHP.currentHealth == 1 && !isQTEActive)
        {
            popupCanvas.SetActive(true);  // Show the popup when player is in range
            Debug.Log("Popup should display. Player in range and Boss Health: " + bossHP.currentHealth);

            // When 'E' is pressed, start the QTE
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed. Starting QTE.");
                isQTEActive = true;
                bossQTE.StartQTE();  // Start the QTE
                popupCanvas.SetActive(false);  // Hide the popup after the QTE starts
            }
        }
        else
        {
            popupCanvas.SetActive(false);  // Hide the popup when not in range or QTE is active
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
