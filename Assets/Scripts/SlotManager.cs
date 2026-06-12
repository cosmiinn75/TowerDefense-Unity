using Assets.FantasyTowerDefense.Scripts.Demo;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Collections;

public class SlotManager : MonoBehaviour
{
    private GameObject tower;
    private GameObject currentUnderConstruction;
    public Sprite lockedRenderer;
    private Sprite unlockedRenderer;
    private SpriteRenderer myRenderer; //Object renderer
    public Color hoverColor;
    private Color baseColor;
    public int unlockCost;
    private int changeElementSpent = 0;
    private bool isPlaced;
    private int currentLevel = 0;
    public bool isLocked = false;
    [Header("UI Menu")]
    public GameObject buildMenu;
    public GameObject upgradeSellMenu;
    private GameObject currentMenu;
    public GameObject sellMenu;
    public Canvas mainCanvas;
    private List<GameObject> towerType;
    private UpgradeSellMenu currentUpgradeSellMenu;
    public GameObject dontHaveEnoughGoldText;
    public GameObject chooseMagicMenu;
    public GameObject unlockMenu;
    [Header("Tower Prefabs")]
    public List<GameObject> cannonTowers;
    public List<GameObject> archerTowers;
    public List<GameObject> magicTowers;
    public GameObject underConstructionPrefab;

    private float timeTillClosedMenu = 4f;
    private Coroutine menuTimerCoroutine;


    private GameObject localBuildMenu;
    private GameObject localUpgradeSellMenu;
    private GameObject localSellMenu;
    private GameObject localChooseMagicMenu;
    private GameObject localUnlockMenu;

    private void Awake()
    {
        if (buildMenu && mainCanvas)
        {
            localBuildMenu = Instantiate(buildMenu, mainCanvas.transform,false);
            localBuildMenu.transform.SetAsFirstSibling();
            localBuildMenu.SetActive(false);
        }
        if (upgradeSellMenu && mainCanvas)
        {
            localUpgradeSellMenu = Instantiate(upgradeSellMenu, mainCanvas.transform,false);
            localUpgradeSellMenu.transform.SetAsFirstSibling();
            localUpgradeSellMenu.SetActive(false);
        }
        if (chooseMagicMenu && mainCanvas)
        {
            localChooseMagicMenu = Instantiate(chooseMagicMenu, mainCanvas.transform,false);
            localChooseMagicMenu.transform.SetAsFirstSibling();
            localChooseMagicMenu.SetActive(false);
        }
        if (sellMenu && mainCanvas)
        {
            localSellMenu = Instantiate(sellMenu, mainCanvas.transform,false);
            localSellMenu.transform.SetAsFirstSibling();
            localSellMenu.SetActive(false);
        }
        if (unlockMenu && mainCanvas)
        {
            localUnlockMenu = Instantiate(unlockMenu, mainCanvas.transform,false);
            localUnlockMenu.transform.SetAsFirstSibling();
            localUnlockMenu.SetActive(false);
        }
    }

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        baseColor = myRenderer.color;
        isPlaced = false;

        if(dontHaveEnoughGoldText != null)
        {
            dontHaveEnoughGoldText.SetActive(false);
        }
        unlockedRenderer = myRenderer.sprite;
        if (isLocked) {
            myRenderer.sprite = lockedRenderer;
        }
        else
        {
            myRenderer.sprite = unlockedRenderer;
        }

    

    }


    private void OnMouseEnter()
    {

        if (currentUnderConstruction != null)
        {
            var sr = currentUnderConstruction.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = hoverColor;
        }
  
        else if (!isPlaced)
        {
            myRenderer.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {

        if (currentUnderConstruction != null)
        {
            var sr = currentUnderConstruction.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.white;
        }

        myRenderer.color = baseColor;
    }

    private void OnMouseDown()
    {

        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.isPaused)
            {
                return;
            }
        }
        if (currentMenu != null)
        {
            CloseCurrentMenu();
            return;
        }

        if(Time.timeScale == 0)
        {
            return;
        }



        HandleMenus();
    }
    void HandleMenus() {

        if (isLocked) {
            OpenUnlockMenu();
            return;
        }

        if (!isPlaced)
        {
            OpenBuildMenu();
        }
        else
        {
            MagicTower magicTower = tower.GetComponent<MagicTower>();
            if (magicTower != null)
            {

                if (currentLevel == 2)
                {
                    OpenChooseMagicMenu();
                }
                else
                {
                    OpenUpgradeSellMenu();
                }

            }
            else if (currentLevel == 2)
            {
                OpenSellMenu();
            }
            else
            {
                OpenUpgradeSellMenu();
            }

        }

        if (currentMenu != null)
        {
            ResetMenuTimer();
        }

    }

    public void OnUnlock()
    {
        if (CurrencyManager.Instance.currentGold < unlockCost) {
            NotEnoughGold();
            return;
        }
        CurrencyManager.Instance.TrySpendGold(unlockCost);
        myRenderer.sprite = unlockedRenderer;
        isLocked = false;
        CloseCurrentMenu();

    }
    public void OnButtonClicked(string towerToBuild)
    {
        if (isPlaced) return; 

        if (towerToBuild == nameof(cannonTowers)) towerType = cannonTowers;
        else if (towerToBuild == nameof(archerTowers)) towerType = archerTowers;
        else if (towerToBuild == nameof(magicTowers)) towerType = magicTowers;

        GameObject prefabToBuild = towerType[0];
        Tower towerScript = prefabToBuild.GetComponent<Tower>();
        //If you can afford
        if (towerScript.Cost <= CurrencyManager.Instance.currentGold)
        {
            CurrencyManager.Instance.TrySpendGold((int)towerScript.Cost);

        
            if (currentUnderConstruction != null) Destroy(currentUnderConstruction);
            PlayBuySound();
            tower = Instantiate(prefabToBuild, transform.position, Quaternion.identity);
            isPlaced = true;
            myRenderer.enabled = false; 
            CloseCurrentMenu();
        }
        else
        {
            NotEnoughGold();
            return;
        }

    }

    public void OnUpgrade()
    {
        if (currentLevel >= towerType.Count - 1) return; //If there isn't an available upgrade

        GameObject nextLevelPrefab = towerType[currentLevel + 1];
        Tower towerScript = nextLevelPrefab.GetComponent<Tower>();

        if (towerScript.Cost <= CurrencyManager.Instance.currentGold)
        {
            CurrencyManager.Instance.TrySpendGold((int)towerScript.Cost);
            Destroy(tower);
            currentLevel++;
            PlayBuySound();
            tower = Instantiate(towerType[currentLevel], transform.position, Quaternion.identity);

            if (currentUpgradeSellMenu != null)
                currentUpgradeSellMenu.UpdateUI(GetNextUpgradeCost()); // Shows next cost

            CloseCurrentMenu();
        }
        else
        {
            NotEnoughGold();
            return;
        }
   
    }

    public void OnSell()
    {
        float totalCost = 0;
        totalCost += changeElementSpent;
        for (int i = 0; i <= currentLevel; i++)
        {
            totalCost += (int)towerType[i].GetComponent<Tower>().Cost;
        }
        PlaySellSound();
        CurrencyManager.Instance.AddGold(Mathf.RoundToInt(totalCost * 0.8f / 10f) * 10);
        changeElementSpent = 0;
            
        Destroy(tower);
        tower = null;
        currentLevel = 0;
        isPlaced = false;

        myRenderer.enabled = true;
        myRenderer.color = baseColor;

        if (currentUnderConstruction != null) 
        { 
            Destroy(currentUnderConstruction); 
        }
        currentUnderConstruction = Instantiate(underConstructionPrefab, transform.position, Quaternion.identity);
        CloseCurrentMenu();
    
    }

    public void OnSelectElement(string elementType)
    {
       

        var script = tower.GetComponent<MagicTower>();
        int cost = 200;
        if(script.currentElement == elementType)
        {
            CloseCurrentMenu();
            return;
        }

        if (cost <= CurrencyManager.Instance.currentGold)
        {

            if (script != null)
            {
                CurrencyManager.Instance.TrySpendGold(cost);
                changeElementSpent += cost;
                script.ChangeElement(elementType);
             
            }

            CloseCurrentMenu();
        }
        else
        {
            NotEnoughGold();
            return;
        }
    }

    private void ResetMenuTimer()
    {
        if(menuTimerCoroutine != null)
        {
            StopCoroutine(menuTimerCoroutine);
        }
        menuTimerCoroutine = StartCoroutine(CloseMenuAfterTime());
    }
    private IEnumerator CloseMenuAfterTime()
    {
        yield return new WaitForSeconds(timeTillClosedMenu);
        CloseCurrentMenu();
        menuTimerCoroutine = null;
    }
    private void CloseCurrentMenu()
    {
        if (currentMenu == null) return;
        MenuFadeAnimation anim = currentMenu.GetComponent<MenuFadeAnimation>();
        if (anim != null) anim.CloseMenu();
        else currentMenu.SetActive(false);
        currentMenu = null;
  
    }

    public void OpenBuildMenu()
    {
        PositionAndShowMenu(localBuildMenu);
        currentMenu.GetComponent<BuildMenu>().SetSlot(this);
    }

    public void OpenUnlockMenu()
    {
        PositionAndShowMenu(localUnlockMenu);
        currentMenu.GetComponent<UnlockMenu>().SetSlot(this);
    }

    public void OpenSellMenu()
    {
        PositionAndShowMenu(localSellMenu);
        currentMenu.GetComponent<SellMenu>().SetSlot(this);
    }

    public void OpenUpgradeSellMenu()
    {
        PositionAndShowMenu(localUpgradeSellMenu);
        var script = currentMenu.GetComponent<UpgradeSellMenu>();
        script.SetSlot(this);
        currentUpgradeSellMenu = script;
    }

    public void OpenChooseMagicMenu()
    {
        PositionAndShowMenu(localChooseMagicMenu);
        currentMenu.GetComponent<ChooseMagicMenu>().SetSlot(this);
    }
    public void SetUpgradeSellMenu(UpgradeSellMenu menu) { currentUpgradeSellMenu = menu; }
    public int GetNextUpgradeCost()
    {
        if (towerType == null || currentLevel >= towerType.Count - 1) return 0;
        return (int)towerType[currentLevel + 1].GetComponent<Tower>().Cost;
    }

    private void PlayBuySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buyClip);
        }
    }
    private void PlaySellSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sellClip);
        }

    }
    private void NotEnoughGold() {
        dontHaveEnoughGoldText.SetActive(true);
        dontHaveEnoughGoldText.GetComponent<TextFadeAnimation>()?.TriggerAnimation();
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.notEnoughGoldClip);
        }
        CloseCurrentMenu();
    }
    private void PositionAndShowMenu(GameObject menu)
    {
        currentMenu = menu;
        currentMenu.transform.position = transform.position;
        Vector3 localPos = currentMenu.transform.localPosition;
        localPos.z = -5f;
        currentMenu.transform.localPosition = localPos;
        currentMenu.SetActive(true);
    }
}