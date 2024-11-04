using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad = "Level1"; 
    public float fadeDuration = 1.0f; 
    public Image fadePanel; 

    private void Start()
    {
        fadePanel.color = new Color(0, 0, 0, 1); 
        StartCoroutine(FadeIn()); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndChangeScene());
        }
    }

    private IEnumerator FadeOutAndChangeScene()
    {
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null; 
        }

        SceneManager.LoadScene(sceneToLoad); 
    }

    private IEnumerator FadeIn()
    {
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null; 
        }
    }
}
