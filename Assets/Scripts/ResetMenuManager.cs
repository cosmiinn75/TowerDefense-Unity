using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResetMenuManager : MonoBehaviour
{
    [Header("Backend")]
    [SerializeField] private string baseURL = "http://localhost:8080";

    [Header("Campaign")]
    [SerializeField] private int maxCampaignLevel = 10;

    private const string JwtTokenKey = "jwt_token";
    private const string IsGuestKey = "is_guest";
    private const string GuestMaxLevelUnlockedKey = "guest_max_level_unlocked";
    private const string GuestLevelStarsPrefix = "guest_level_";
    private const string GuestLevelStarsSuffix = "_stars";

    public void DeleteAllProgress()
    {
        if (GameSession.IsGuest || PlayerPrefs.GetInt(IsGuestKey, 0) == 1)
        {
            ResetGuestProgress();
            ReloadCurrentScene();
            return;
        }

        StartCoroutine(ResetProgressCoroutine());
    }

    private IEnumerator ResetProgressCoroutine()
    {
        string token = GameSession.Token;

        if (string.IsNullOrWhiteSpace(token))
        {
            token = PlayerPrefs.GetString(JwtTokenKey, "");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogError("Cannot reset progress. Missing JWT token.");
            yield break;
        }

        UnityWebRequest request =
            new UnityWebRequest(baseURL + "/api/player/progress/reset", "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Failed to reset progress: " +
                request.responseCode + " " +
                request.downloadHandler.text
            );

            yield break;
        }

        PlayerProgressResponse resetProgress =
            JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);

        if (resetProgress == null || resetProgress.levels == null)
        {
            Debug.LogError("Invalid reset response: " + request.downloadHandler.text);
            yield break;
        }

        GameSession.Progress = resetProgress;

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SetProgressFromBackend(resetProgress);
        }

        Debug.Log("Progress reset. Max unlocked level: " + resetProgress.maxLevelUnlocked);

        ReloadCurrentScene();
    }

    private void ResetGuestProgress()
    {
        GameSession.IsGuest = true;
        GameSession.Token = "";
        GameSession.Username = "Guest";
        GameSession.Progress = null;
        GameSession.SelectedLevelNumber = 0;

        PlayerPrefs.SetInt(IsGuestKey, 1);
        PlayerPrefs.DeleteKey(GuestMaxLevelUnlockedKey);

        for (int i = 1; i <= maxCampaignLevel; i++)
        {
            PlayerPrefs.DeleteKey(GetGuestStarsKey(i));
        }

        PlayerPrefs.Save();

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.SyncFromGuestProgress();
        }

        Debug.Log("Guest progress reset.");
    }

    private string GetGuestStarsKey(int levelNumber)
    {
        return GuestLevelStarsPrefix + levelNumber + GuestLevelStarsSuffix;
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}