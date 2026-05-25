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

    private int selectedLevelIndex;
    private void Start()
    {
        startLevelPanel.gameObject.SetActive(false);
        for(int i = 0; i< levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            levelButtons[i].onClick.AddListener(() => OpenStartLevelPanel(levelNum));
        }

    }
    public void OpenStartLevelPanel(int levelNum)
    {
        int maxLevel = MainGameManager.Instance != null ? MainGameManager.Instance.maxLevelReached : 1;
        if (levelNum <= maxLevel)
        {
            selectedLevelIndex = levelNum;
            string key = "StarsUnlocked" + levelNum;
            float fillAmonut = PlayerPrefs.GetFloat(key);
            filledStarImage.fillAmount = fillAmonut;
            levelText.text = "Level " + levelNum.ToString();
            startLevelPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("N-am deblocat nivelul");
        }
    }

    public void OnPlay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + selectedLevelIndex);
    }

   public void OnBack()
    {
        startLevelPanel.gameObject.SetActive(false);


    }

}
