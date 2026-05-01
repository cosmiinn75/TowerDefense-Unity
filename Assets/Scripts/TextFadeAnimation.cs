using System.Collections;
using TMPro;
using UnityEngine;

public class TextFadeAnimation : MonoBehaviour
{

    private TextMeshProUGUI textMeshPro;
    public float fadeInDuration = 0.2f;
    public float displayDuration = 0.2f;
    public float fadeOutDuration = 0.2f;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        Color color = textMeshPro.color;
        color.a = 0f;
        textMeshPro.color = color;
    }


    public void TriggerAnimation()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {

        yield return StartCoroutine(FadeTo(1f, fadeInDuration));

        yield return new WaitForSeconds(displayDuration);


        yield return StartCoroutine(FadeTo(0f, fadeOutDuration));

        gameObject.SetActive(false);
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = textMeshPro.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            Color color = textMeshPro.color;
            color.a = newAlpha;
            textMeshPro.color = color;

            yield return null;
        }


        Color finalColor = textMeshPro.color;
        finalColor.a = targetAlpha;
        textMeshPro.color = finalColor;
    }
}