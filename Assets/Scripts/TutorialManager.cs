using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int currentPage = 0;
    [SerializeField] Button backButton;
    [SerializeField] Button forwardButton;
    [SerializeField] Button XButton;

    private void OnEnable()
    {
        currentPage = 0;
        OpenCorrectPage();
    }
    public void OnX()
    {
        gameObject.SetActive(false);
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
    void OpenCorrectPage()
    {
        backButton.gameObject.SetActive(currentPage > 0);
        forwardButton.gameObject.SetActive(currentPage < pages.Length-1);
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(currentPage == i);
        }
    }
}
