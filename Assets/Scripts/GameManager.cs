using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameOver;
    [Header("Panels")]
    public GameObject winLosePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public bool win;
    private bool isPaused = false;
    public GameObject stars;
    private int currentLevelIndex;
    private void Awake()
    {
        if (Instance == null) { Instance = this;
            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        else { Destroy(gameObject); }
        
    }
    private void Start()
    {
        AudioManager.Instance.StopMusic();
        winLosePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        gameOver = false;
      
    }

    public void OpenWinLosePanel() {

        winLosePanel.SetActive(true);
        if (win)
        {
            Stars();
            winPanel.SetActive(true);
            if(MainGameManager.Instance != null)
            {
                MainGameManager.Instance.UnlockNextLevel(currentLevelIndex);
            }
        }
        else {
            losePanel.SetActive(true);
        }
        
    }

    public void OnRetry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnNextLevel() {
        Time.timeScale = 1f;


        int nextIndex = currentLevelIndex + 1;

  
        if (nextIndex< SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("LastAccesedLevel", nextIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene(nextIndex);
        }
    }
    private void Stars()
    {
        if (stars == null) return;
        var starImage = stars.GetComponent<Image>();
        if (starImage == null) return;
        if (DamageKingTower.Instance?._kingMonster != null)
        {
          
            float currentHealth = DamageKingTower.Instance._kingMonster._health;
            float maxHealth = DamageKingTower.Instance._kingMonster.Health;
           
            float percentage = currentHealth / maxHealth;
            float currentFill = 0.0f;
            if (percentage >= 0.8f)
            {
                currentFill = 1f;
            }

            else if (percentage >= 0.4f)
            {
                currentFill = 0.67f;
            }
            else
            {
                currentFill = 0.34f;
            }
            starImage.fillAmount = currentFill;

            string saveKey = "StarsUnlocked" + currentLevelIndex;
            float filledSave = PlayerPrefs.GetFloat(saveKey, 0f);

            if(currentFill > filledSave)
            {
                PlayerPrefs.SetFloat(saveKey, currentFill);
                PlayerPrefs.Save();
            }

        }
    }
    public void OnBack()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void OnWorldMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WorldMap");
        PlayerPrefs.SetInt("LastAccesedLevel", currentLevelIndex);
        PlayerPrefs.Save();
    }


    public void OnPause(InputValue inputValue) {

        if (inputValue.isPressed && !gameOver) {
            
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            if(isPaused)
            {
                Debug.Log("Pauza");
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

    }

    public void OnResume()
    {
        Debug.Log("Apasat ce are");
        Time.timeScale = 1.0f;
        isPaused = false;
        pausePanel.SetActive(false);
        
    }

}
