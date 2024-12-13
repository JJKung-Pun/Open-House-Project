using UnityEngine;
using TMPro;

public class UIFollowBoss : MonoBehaviour
{
    public Transform bossTransform; // Assign the boss's transform
    public Camera mainCamera; // Assign the main camera
    public Vector3 offset = new Vector3(0, 50, 0); // Offset in pixels for UI positioning

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (bossTransform != null && mainCamera != null)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(bossTransform.position + new Vector3(0, 2, 0));
            rectTransform.position = screenPos + offset;
        }
    }
}
