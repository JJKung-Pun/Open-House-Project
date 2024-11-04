using UnityEngine;

public class BossWarp : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false); // Ensure the GameObject is inactive at the start
    }

    public void OnBossDeath()
    {
        gameObject.SetActive(true); // Activate the GameObject when the boss dies
    }
}
