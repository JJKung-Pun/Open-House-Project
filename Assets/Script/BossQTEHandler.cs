using UnityEngine;

public class BossQTEHandler : MonoBehaviour
{
    public QTEManager qteManager;
    public Boss1HP bossHealth;

    private void Start()
    {
        // Subscribe to QTE events
        if (qteManager != null)
        {
            qteManager.OnQTESuccess += HandleQTESuccess;
            qteManager.OnQTEFail += HandleQTEFail;
        }

        if (bossHealth == null)
        {
            Debug.LogError("BossQTEHandler: Boss1HP reference is missing!");
        }

        if (qteManager == null)
        {
            Debug.LogError("BossQTEHandler: QTEManager reference is missing!");
        }
    }

    /// <summary>
    /// Initiates a QTE when called.
    /// </summary>
    public void TriggerQTE()
    {
        if (qteManager != null)
        {
            qteManager.StartRandomQTE();
        }
    }

    /// <summary>
    /// Handles the QTE success event by setting the boss's health to 0.
    /// </summary>
    private void HandleQTESuccess()
    {
        Debug.Log("QTE Success! Boss will be defeated.");
        if (bossHealth != null)
        {
            bossHealth.SetHealth(0); // Set health to 0 to trigger death
        }
        else
        {
            Debug.LogWarning("BossQTEHandler: Boss1HP reference is missing!");
        }
    }

    /// <summary>
    /// Handles the QTE failure event by healing the boss to maximum health.
    /// </summary>
    private void HandleQTEFail()
    {
        Debug.Log("QTE Failed! Boss is healed to full health.");
        if (bossHealth != null)
        {
            bossHealth.ResetHealth(); // Heal boss to max
        }
        else
        {
            Debug.LogWarning("BossQTEHandler: Boss1HP reference is missing!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (qteManager != null)
        {
            qteManager.OnQTESuccess -= HandleQTESuccess;
            qteManager.OnQTEFail -= HandleQTEFail;
        }
    }
}
