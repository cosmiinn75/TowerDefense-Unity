using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    [SerializeField] private Button backButton;
    [SerializeField] private Button forwardButton;
    [SerializeField] private Button XButton;

    private int currentPage = 0;

    private const string TutorialVersionKey = "TutorialVersionSeen";
    private const int CurrentTutorialVersion = 10;

    public event Action OnTutorialClosed;

    private void OnEnable()
    {
        currentPage = 0;
        OpenCorrectPage();
    }

    public static bool HasSeenCurrentTutorial()
    {
        return PlayerPrefs.GetInt(TutorialVersionKey, 0) >= CurrentTutorialVersion;
    }

    public void OnX()
    {
        PlayerPrefs.SetInt(TutorialVersionKey, CurrentTutorialVersion);
        PlayerPrefs.Save();

        gameObject.SetActive(false);

        OnTutorialClosed?.Invoke();
    }

    public void OnBack()
    {
        if (currentPage > 0)
        {
            currentPage--;
            OpenCorrectPage();
        }
    }

    public void OnForward()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            OpenCorrectPage();
        }
    }

    private void OpenCorrectPage()
    {
        backButton.gameObject.SetActive(currentPage > 0);
        forwardButton.gameObject.SetActive(currentPage < pages.Length - 1);

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(currentPage == i);
        }
    }
}