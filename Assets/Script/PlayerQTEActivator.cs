using UnityEngine;
using TMPro;

public class PlayerQTEActivator : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activationRange = 2f; // Distance within which player can initiate QTE
    public KeyCode activationKey = KeyCode.E; // Key to press to start QTE

    [Header("References")]
    public QTEManager qteManager;
    public BossQTEHandler bossQTEHandler;
    public CanvasGroup qtePromptCanvasGroup; // "Press E to QTE" prompt

    private Transform bossTransform;
    private bool isInRange = false;

    void Start()
    {
        // Find the boss in the scene by tag
        GameObject boss = GameObject.FindWithTag("Boss"); // Ensure your boss GameObject has the tag "Boss"
        if (boss != null)
            bossTransform = boss.transform;
        else
            Debug.LogError("PlayerQTEActivator: No GameObject with tag 'Boss' found!");

        // Hide the QTE prompt initially
        if (qtePromptCanvasGroup != null)
        {
            qtePromptCanvasGroup.alpha = 0f;
            qtePromptCanvasGroup.interactable = false;
            qtePromptCanvasGroup.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (bossTransform == null || bossQTEHandler == null || bossQTEHandler.bossHealth == null) return;

        // Calculate distance to boss
        float distance = Vector2.Distance(transform.position, bossTransform.position);

        // Check if player is within activation range and boss's health is below 10
        if (distance <= activationRange && bossQTEHandler.bossHealth.currentHealth < 10 && bossQTEHandler.bossHealth.currentHealth > 0)
        {
            if (!isInRange)
            {
                isInRange = true;
                ShowQTEPrompt();
            }

            // Check for activation key press
            if (Input.GetKeyDown(activationKey))
            {
                StartQTE();
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                HideQTEPrompt();
            }
        }
    }

    private void StartQTE()
    {
        if (qteManager != null && bossQTEHandler != null)
        {
            bossQTEHandler.TriggerQTE();
            HideQTEPrompt();
        }
    }

    private void ShowQTEPrompt()
    {
        if (qtePromptCanvasGroup != null)
        {
            qtePromptCanvasGroup.alpha = 1f;
            qtePromptCanvasGroup.interactable = true;
            qtePromptCanvasGroup.blocksRaycasts = true;
        }
    }

    private void HideQTEPrompt()
    {
        if (qtePromptCanvasGroup != null)
        {
            qtePromptCanvasGroup.alpha = 0f;
            qtePromptCanvasGroup.interactable = false;
            qtePromptCanvasGroup.blocksRaycasts = false;
        }
    }
}
