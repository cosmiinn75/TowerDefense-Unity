
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResetMenuManager : MonoBehaviour
{

    [Header("Backend")]
    [SerializeField] private string baseURL = "http://localhost:8080"; 
    public void DeleteAllProgress() {
        StartCoroutine(ResetProgressCoroutine());
    }

    private IEnumerator ResetProgressCoroutine()
    {
        string token = GameSession.Token;

        if (string.IsNullOrWhiteSpace(token))
        {
            token = PlayerPrefs.GetString("jwt_token", "");
        }
        if (string.IsNullOrWhiteSpace(token))
        {
            Debug.LogError("Cannot reset progress. Missing JWT token.");
            yield break;
        }

        UnityWebRequest request = new UnityWebRequest(baseURL + "/api/player/progress/reset", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                "Failed to reset progress: "
                + request.responseCode + " "
                + request.downloadHandler.text
            );

            yield break;
        }
        PlayerProgressResponse resetProgress = JsonUtility.FromJson<PlayerProgressResponse>(request.downloadHandler.text);
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
