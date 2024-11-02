using System.Collections;
using UnityEngine;

public class Boss1QTE : MonoBehaviour
{
    private bool isInQTE = false;
    private KeyCode qteKey;
    private float qteDuration = 5f;
    private float qteCooldown = 2f;
    private float qteCooldownTimer = 0f;

    void Update()
    {
        if (qteCooldownTimer > 0)
        {
            qteCooldownTimer -= Time.deltaTime;
        }
        if (isInQTE && Input.GetKeyDown(qteKey))
        {
            EndQTE(true);
        }
    }

    public void StartQTE()
    {
        if (isInQTE || qteCooldownTimer > 0) return;
        isInQTE = true;
        gameObject.GetComponent<Boss1HP>().enabled = false;
        qteKey = GetRandomKey();
        StartCoroutine(QTECoroutine());
    }

    private IEnumerator QTECoroutine()
    {
        yield return new WaitForSeconds(qteDuration);
        EndQTE(false);
    }

    public void EndQTE(bool success)
    {
        isInQTE = false;
        gameObject.GetComponent<Boss1HP>().enabled = true;
        qteCooldownTimer = qteCooldown;
    }

    public bool IsInQTE()
    {
        return isInQTE;
    }

    private KeyCode GetRandomKey()
    {
        int randomValue = Random.Range(0, 16);
        return (KeyCode)(randomValue + (int)KeyCode.A);
    }
}
