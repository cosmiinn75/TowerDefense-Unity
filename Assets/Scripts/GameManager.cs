using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameOver;

    [HideInInspector] public float currentSpeed = 1f;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Button speedButton;

    [Header("Panels")]
    public GameObject winLosePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    public bool win;

    [HideInInspector] public bool isPaused = false;

    [Header("Stars")]
    public GameObject stars;

    [Header("Level")]
    public int currentLevelIndex;

    [Header("Backend")]
    [SerializeField] private string baseURL = "http://localhost:8080";

    [Header("Progress Save State")]
    [HideInInspector] public bool canLeaveWinScreen = true;

    private const string JwtTokenKey = "jwt_token";
    private const string IsGuestKey = "is_guest";

    private bool progressSaveStarted;

    [Serializable]
    public class UpdateLevelRequest
    {
        public int stars;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentLevelIndex = GetCurrentLevelNumber();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SyncGuestStateFromPlayerPrefs();

        if (speedText != null)
        {
            speedText.text = "1.0x";
        }

        win = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameClip);
        }

        if (winLosePanel != null)
        {
            winLosePanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        isPaused = false;
        Time.timeScale = 1f;
        gameOver = false;

        progressSaveStarted = false;
        canLeaveWinScreen = true;
    }

    private void SyncGuestStateFromPlayerPrefs()
    {
        bool savedGuestMode = PlayerPrefs.GetInt(IsGuestKey, 0) == 1;

        if (savedGuestMode)
        {
            GameSession.IsGuest = true;
            GameSession.Token = "";
            GameSession.Username = "Guest";
            GameSession.Progress = null;
        }
    }

    public void OpenWinLosePanel()
    {
        if (winLosePanel != null)
        {
            winLosePanel.SetActive(true);
        }

        if (win)
        {
            int earnedStars = Stars();

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.winClip);
            }

            if (!progressSaveStarted)
            {
                progressSaveStarted = true;
                canLeaveWinScreen = false;

                Debug.Log("Started saving progress.");

                if (GameSession.IsGuest)
                {
                    SaveLevelResultToPlayerPrefs(currentLevelIndex, earnedStars);
                    FinishProgressSave();
                }
                else
                {
                    StartCoroutine(SaveLevelResultToBackend(currentLevelIndex, earnedStars));
                }
            }
        }
        else
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.loseClip);
            }

            if (losePanel != null)
            {
                losePanel.SetActive(true);
            }
        }
    }

    private int Stars()
    {
        if (stars == null)
        {
            return 1;
        }

        Image starImage = stars.GetComponent<Image>();

        if (starImage == null)
        {
            return 1;
        }

        int earnedStars = 1;

        if (DamageKingTower.Instance?._kingMonster != null)
        {
            float currentHealth = DamageKingTower.Instance._kingMonster._health;
            float maxHealth = DamageKingTower.Instance._kingMonster.Health;

            if (maxHealth > 0)
            {
                float percentage = currentHealth / maxHealth;

                if (percentage >= 0.8f)
                {
                    earnedStars = 3;
                }
                else if (percentage >= 0.4f)
                {
                    earnedStars = 2;
                }
                else
                {
                    earnedStars = 1;
                }
            }
        }

        starImage.fillAmount = StarsToFillAmount(earnedStars);

        return earnedStars;
    }

    private float StarsToFillAmount(int stars)
    {
        if (stars >= 3) return 1f;
        if (stars == 2) return 0.67f;
        if (stars == 1) return 0.34f;

        return 0f;
    }

    private int GetCurrentLevelNumber()
    {
        if (GameSession.SelectedLevelNumber > 0)
        {
            return GameSession.SelectedLevelNumber;
        }

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level"))
        {
            string numberText = sceneName.Replace("Level", "");

            if (int.TryParse(numberText, out int levelNumber))
            {
                return levelNumber;
            }
        }

        Debug.LogWarning("Could not detect level number. Defaulting to Level 1.");
        return 1;
    }

    private void SaveLevelResultToPlayerPrefs(int levelNumber, int earnedStars)
    {
        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SaveGuestLevelResult(levelNumber, earnedStars);
            Debug.Log("Guest progress saved locally.");
            return;
        }

        Debug.LogWarning("MainGameManager missing. Guest progress was not saved.");
    }

    private IEnumerator SaveLevelResultToBackend(int levelNumber, int earnedStars)
    {
        if (GameSession.IsGuest)
        {
            Debug.Log("Guest mode active. Skipping backend progress save.");
            FinishProgressSave();
            yield break;
        }

        string token = GameSession.Token;

        if (string.IsNullOrWhiteSpace(token))
        {
            token = PlayerPrefs.GetString(JwtTokenKey, "");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogError("Cannot save progress. Missing JWT token.");
            FinishProgressSave();
            yield break;
        }

        UpdateLevelRequest updateRequest = new UpdateLevelRequest
        {
            stars = earnedStars
        };

        string json = JsonUtility.ToJson(updateRequest);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request =
            new UnityWebRequest(baseURL + "/api/player/levels/" + levelNumber, "PUT");

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        Debug.Log("Saving progress for Level " + levelNumber + " with stars: " + earnedStars);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Failed to save progress: " +
                request.responseCode + " " +
                request.downloadHandler.text
            );

            FinishProgressSave();
            yield break;
        }

        PlayerProgressResponse updatedProgress =
            JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);

        if (updatedProgress == null || updatedProgress.levels == null)
        {
            Debug.LogError("Invalid progress response: " + request.downloadHandler.text);
            FinishProgressSave();
            yield break;
        }

        GameSession.Progress = updatedProgress;

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SetProgressFromBackend(updatedProgress);
        }

        Debug.Log("Progress saved. Max unlocked level: " + updatedProgress.maxLevelUnlocked);

        FinishProgressSave();
    }

    private void FinishProgressSave()
    {
        canLeaveWinScreen = true;
        Debug.Log("Finished saving progress. Player can now leave win screen.");
    }

    public void OnPause(InputValue inputValue)
    {
        if (SceneTransitionerManager.Instance != null)
        {
            if (SceneTransitionerManager.Instance.isTransitioning)
            {
                return;
            }
        }

        if (inputValue.isPressed && !gameOver)
        {
            isPaused = !isPaused;

            PauseManager pauseManager = GetComponent<PauseManager>();

            if (pauseManager != null)
            {
                pauseManager.OnBack();
            }

            if (pausePanel != null)
            {
                pausePanel.SetActive(isPaused);
            }

            if (isPaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = currentSpeed;
            }
        }
    }

    public void OnSpeed()
    {
        if (!isPaused)
        {
            currentSpeed += 0.25f;

            if (currentSpeed > 1.5f)
            {
                currentSpeed = 1f;
            }

            if (speedText != null)
            {
                speedText.text = currentSpeed.ToString("0.00") + "x";
            }

            Time.timeScale = currentSpeed;
        }
    }
}