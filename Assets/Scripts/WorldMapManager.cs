using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject startLevelPanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image filledStarImage;
    [SerializeField] private Button backArrow;

    [Header("Map")]
    [SerializeField] private ScrollRect mapScrollRect;
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform mapViewport;
    [SerializeField] private RectTransform[] levelPositions;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 1f;
    [SerializeField]
    private AnimationCurve animationCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    public int lastLevelIndex;

    private int selectedLevelIndex;
    private Coroutine scrollCoroutine;

    private event Action<int> OnMapInitialized;

    private void Start()
    {
  
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundClip);
        }

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

        KeepAllLevelButtonsClickable();

        StartCoroutine(InitializeMap());
    }

    private IEnumerator InitializeMap()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        int levelToFocus = 1;


        if (GameSession.Progress != null)
        {
            levelToFocus = GameSession.Progress.maxLevelUnlocked;
        }

        levelToFocus = Mathf.Clamp(levelToFocus, 1, levelPositions.Length);

        OnMapInitialized?.Invoke(levelToFocus);
    }

    private void KeepAllLevelButtonsClickable()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                levelButtons[i].interactable = true;
            }
        }
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

        Vector2 targetPos = CalculateCenteredPosition(target);
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
        int maxLevelUnlocked = GetMaxLevelUnlocked();


        if (levelNum > maxLevelUnlocked)
        {
            return;
        }

        selectedLevelIndex = levelNum;


        int stars = GetStarsForLevel(levelNum);
        filledStarImage.fillAmount = StarsToFillAmount(stars);

        levelText.text = $"Level {levelNum}";

        backArrow.gameObject.SetActive(false);
        startLevelPanel.SetActive(true);

        FocusOnLevel(levelNum);
    }

    public void OnPlay()
    {
        Time.timeScale = 1f;


        GameSession.SelectedLevelNumber = selectedLevelIndex;

        if (SceneTransitionerManager.Instance != null)
        {
            SceneTransitionerManager.Instance.LoadScene("Level" + selectedLevelIndex);
        }
        else
        {
            SceneManager.LoadScene("Level" + selectedLevelIndex);
        }
    }

    public void OnBack()
    {
        backArrow.gameObject.SetActive(true);
        startLevelPanel.SetActive(false);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private int GetMaxLevelUnlocked()
    {
        if (GameSession.Progress == null)
        {
            return 1;
        }

        return GameSession.Progress.maxLevelUnlocked;
    }

    private int GetStarsForLevel(int levelNum)
    {
        if (GameSession.Progress == null || GameSession.Progress.levels == null)
        {
            return 0;
        }

        foreach (LevelProgressResponse level in GameSession.Progress.levels)
        {
            if (level.levelNumber == levelNum)
            {
                return level.stars;
            }
        }

        return 0;
    }

    private float StarsToFillAmount(int stars)
    {
        if (stars >= 3) return 1f;
        if (stars == 2) return 0.67f;
        if (stars == 1) return 0.34f;

        return 0f;
    }
}