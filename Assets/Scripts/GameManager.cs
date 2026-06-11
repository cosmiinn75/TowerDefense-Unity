
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameOver;
    private float currentSpeed = 1f;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Button speedButton;
    [Header("Panels")]
    public GameObject winLosePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public bool win;
    [HideInInspector] public bool isPaused = false;
    public GameObject stars;
    public int currentLevelIndex;
    private void Awake()
    {
        if (Instance == null) { Instance = this;
            currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        else { Destroy(gameObject); }
        
    }
    private void Start()
    {
        speedText.text = "1.0x";
        win = false;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.inGameClip);
        }
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
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.winClip);
            }
            if(MainGameManager.Instance != null)
            {
                MainGameManager.Instance.UnlockNextLevel(currentLevelIndex);
            }
        }
        else {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.loseClip);
            }
            losePanel.SetActive(true);
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



    public void OnPause(InputValue inputValue) {

        if(SceneTransitionerManager.Instance!= null)
        {
            if (SceneTransitionerManager.Instance.isTransitioning)
            {
                return;
            }
        }


        if (inputValue.isPressed && !gameOver) {
            
            isPaused = !isPaused;
            PauseManager pauseManager = GetComponent<PauseManager>();
            if(pauseManager != null)
            {
                pauseManager.OnBack();
            }

            pausePanel.SetActive(isPaused);
            if(isPaused)
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
            speedText.text = currentSpeed.ToString() + "x";
            Time.timeScale = currentSpeed;
        }

    }

}
