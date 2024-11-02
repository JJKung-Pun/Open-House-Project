using System.Collections;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public GameObject ePopupCanvas;
    private bool playerInRange = false;
    private Boss1QTE bossQTE;
    private Boss1HP bossHP;

    void Start()
    {
        ePopupCanvas.SetActive(false);
        bossQTE = GetComponent<Boss1QTE>();
        bossHP = GetComponent<Boss1HP>();
    }

    void Update()
    {
        if (playerInRange && bossHP != null && bossHP.currentHealth == 1 && !bossQTE.IsInQTE() && Input.GetKeyDown(KeyCode.E))
        {
            bossQTE.StartQTE();
            ePopupCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && bossHP != null && bossHP.currentHealth == 1 && !bossQTE.IsInQTE())
        {
            playerInRange = true;
            ePopupCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ePopupCanvas.SetActive(false);
        }
    }
}
