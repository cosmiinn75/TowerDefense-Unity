using System.Collections;
using TMPro;
using UnityEngine;

public class TextFadeAnimation : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    public float fadeInDuration = 0.2f;
    public float displayDuration = 0.8f;
    public float fadeOutDuration = 0.2f;

    private Coroutine fadeCoroutine;

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

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeTo(1f, fadeInDuration));

        yield return StartCoroutine(WaitUnscaledButNotPaused(displayDuration));

        yield return StartCoroutine(FadeTo(0f, fadeOutDuration));

        gameObject.SetActive(false);
        fadeCoroutine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = textMeshPro.color.a;
        float time = 0f;

        if (duration <= 0f)
        {
            SetAlpha(targetAlpha);
            yield break;
        }

        while (time < duration)
        {
            if (Time.timeScale > 0f)
            {
                time += Time.unscaledDeltaTime;

                float t = time / duration;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

                SetAlpha(newAlpha);
            }

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private IEnumerator WaitUnscaledButNotPaused(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            if (Time.timeScale > 0f)
            {
                time += Time.unscaledDeltaTime;
            }

            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = textMeshPro.color;
        color.a = alpha;
        textMeshPro.color = color;
    }
}