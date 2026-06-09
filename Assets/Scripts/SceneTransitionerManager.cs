using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionerManager : MonoBehaviour
{
    public static SceneTransitionerManager Instance;


    public bool isTransitioning { get; private set; }
    public CanvasGroup canvasGroup;
    public float duration = 0.6f;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            canvasGroup.alpha = 1f;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    { 
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }
  private  IEnumerator Transition(string sceneName)
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeOut());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeIn());
        isTransitioning = false;
    }

    IEnumerator FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, elapsed / duration);
            yield return null;

        }
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }


    IEnumerator FadeOut()
    {

        canvasGroup.blocksRaycasts = true;
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
    }
}
