using System.Collections;
using UnityEngine;
using TMPro;

public class QTEManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI qteText; // "Press [Key]!" text during QTE
    public TextMeshProUGUI qteActionText; // Optional: Additional QTE action text

    [Header("References")]
    public Transform bossTransform; // Assign the boss's transform
    public Camera mainCamera; // Assign the main camera

    [Header("QTE Settings")]
    public float qteDuration = 2f; // Time allowed to press the key

    // Events to trigger on QTE success or failure
    public delegate void QTEEvent();
    public event QTEEvent OnQTESuccess;
    public event QTEEvent OnQTEFail;

    private bool isQTEActive = false;
    private float timer = 0f;
    private KeyCode currentQTEKey;

    // List of possible QTE keys
    private KeyCode[] qteKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.F, KeyCode.R, KeyCode.T };

    void Start()
    {
        if (qteText != null)
        {
            CanvasGroup cg = qteText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            else
            {
                Debug.LogWarning("QTEManager: QTEText does not have a CanvasGroup component.");
            }
        }

        if (qteActionText != null)
        {
            CanvasGroup cg = qteActionText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            else
            {
                Debug.LogWarning("QTEManager: QTEActionText does not have a CanvasGroup component.");
            }
        }
    }

    /// <summary>
    /// Initiates the QTE with the specified key.
    /// </summary>
    /// <param name="promptKey">The key the player needs to press.</param>
    public void StartQTE(KeyCode promptKey)
    {
        if (!isQTEActive)
            StartCoroutine(QTESequence(promptKey));
    }

    private IEnumerator QTESequence(KeyCode promptKey)
    {
        isQTEActive = true;
        timer = qteDuration;
        currentQTEKey = promptKey;

        // Activate QTE UI elements by setting alpha to 1
        if (qteText != null)
        {
            CanvasGroup cg = qteText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                qteText.text = $"Press [{currentQTEKey}]!";
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        if (qteActionText != null)
        {
            CanvasGroup cg = qteActionText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            // Position QTEActionText near the boss
            if (qteActionText != null && bossTransform != null && mainCamera != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(bossTransform.position + new Vector3(0, 2, 0));
                qteActionText.transform.position = screenPos;
            }

            if (Input.GetKeyDown(currentQTEKey))
            {
                // QTE Success
                OnQTESuccess?.Invoke();
                EndQTE();
                yield break;
            }

            yield return null;
        }

        // QTE Failed
        OnQTEFail?.Invoke();
        EndQTE();
    }

    private void EndQTE()
    {
        isQTEActive = false;

        // Deactivate QTE UI elements by setting alpha to 0
        if (qteText != null)
        {
            CanvasGroup cg = qteText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        if (qteActionText != null)
        {
            CanvasGroup cg = qteActionText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }
    }

    /// <summary>
    /// Optional: Method to start QTE with a random key.
    /// Can be called by other scripts if needed.
    /// </summary>
    public void StartRandomQTE()
    {
        if (qteKeys.Length == 0) return;
        KeyCode randomKey = qteKeys[Random.Range(0, qteKeys.Length)];
        StartQTE(randomKey);
    }
}
