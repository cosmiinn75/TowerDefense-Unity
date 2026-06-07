using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] levelButtons;
    public GameObject startLevelPanel;
    public TextMeshProUGUI levelText;
    public Image filledStarImage;
    public Button backArrow;
    [Header("Map")]
    [SerializeField] ScrollRect mapScrollRect;
    [SerializeField] RectTransform mapContent;
    [SerializeField] RectTransform mapViewport;
    [SerializeField] RectTransform[] levelPositions;

    [Header("Animation")]
    [SerializeField] float animationDuration = 1f;
    public AnimationCurve animationCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    public int lastLevelIndex;

    private int selectedLevelIndex;
    private Coroutine scrollCoroutine;

    public event Action<int> OnMapInitialized;

    private void Start()
    {
        AudioManager.Instance.PlayMusic();
        startLevelPanel.SetActive(false);

        OnMapInitialized += SetLastLevelIndex;
        OnMapInitialized += FocusOnLevel;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            levelButtons[i].onClick.AddListener(() =>
            {
                OpenStartLevelPanel(levelNum);
            });
        }

        StartCoroutine(InitializeMap());
    }

    private IEnumerator InitializeMap()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        int lastSavedLevel =
            PlayerPrefs.GetInt("LastAccessedLevel", 1);

        OnMapInitialized?.Invoke(lastSavedLevel);
    }

    public void SetLastLevelIndex(int level)
    {
        lastLevelIndex = level;
    }

    public void FocusOnLevel(int level)
    {
        if (level < 1 || level > levelPositions.Length)
            return;

        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);

        scrollCoroutine = StartCoroutine(
            AnimateScrollToLevel(levelPositions[level - 1]));
    }

    private IEnumerator AnimateScrollToLevel(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 targetPos =
            CalculateCenteredPosition(target);

        Vector2 startPos = mapContent.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / animationDuration;
            t = animationCurve.Evaluate(t);

            mapContent.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        mapContent.anchoredPosition = targetPos;
        scrollCoroutine = null;
    }

    private Vector2 CalculateCenteredPosition(RectTransform target)
    {
        Vector2 viewportLocalPos =
            (Vector2)mapViewport.InverseTransformPoint(target.position);

        Vector2 contentLocalPos =
            (Vector2)mapViewport.InverseTransformPoint(mapContent.position);

        Vector2 difference =
            contentLocalPos - viewportLocalPos;

        Vector2 desiredPosition =
            mapContent.anchoredPosition + difference;

        float maxX =
            (mapContent.rect.width - mapViewport.rect.width) * 0.5f;

        float maxY =
            (mapContent.rect.height - mapViewport.rect.height) * 0.5f;

        desiredPosition.x =
            Mathf.Clamp(desiredPosition.x, -maxX, maxX);

        desiredPosition.y =
            Mathf.Clamp(desiredPosition.y, -maxY, maxY);

        return desiredPosition;
    }

    public void OpenStartLevelPanel(int levelNum)
    {
        int maxLevel =
            MainGameManager.Instance != null
                ? MainGameManager.Instance.maxLevelReached
                : 1;

        if (levelNum > maxLevel)
            return;

        selectedLevelIndex = levelNum;

        string key = "StarsUnlocked" + levelNum;

        filledStarImage.fillAmount =
            PlayerPrefs.GetFloat(key, 0);

        levelText.text = $"Level {levelNum}";

        backArrow.gameObject.SetActive(false);
        startLevelPanel.SetActive(true);

        FocusOnLevel(levelNum);
    }

    public void OnPlay()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt(
            "LastAccessedLevel",
            selectedLevelIndex);

        PlayerPrefs.Save();

        SceneManager.LoadScene(
            "Level" + selectedLevelIndex);
    }

    public void OnBack()
    {
        backArrow.gameObject.SetActive(true);
        startLevelPanel.SetActive(false);
    }
    public void OnMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}