using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); }
        
    }
    private void Start()
    {
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

            winPanel.SetActive(true);
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
        Debug.Log("--- BUTONUL NEXT LEVEL A FOST APASAT! ---");

        Time.timeScale = 1f;

        int curentIndex = SceneManager.GetActiveScene().buildIndex;
        int urmatorulIndex = curentIndex + 1;

        Debug.Log("Scena curenta are indexul: " + curentIndex);
        Debug.Log("Incerc sa incarc scena cu indexul: " + urmatorulIndex);
        Debug.Log("Total scene in Build Settings: " + SceneManager.sceneCountInBuildSettings);

        if (urmatorulIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(urmatorulIndex);
        }
        else
        {
            Debug.LogWarning("Nu mai exista nicio scena dupa asta in Build Settings!");
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
