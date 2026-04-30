using Assets.FantasyTowerDefense.Scripts.Demo;
using UnityEngine;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    private GameObject tower;
    private GameObject currentUnderConstruction;

    private SpriteRenderer myRenderer; //Object renderer
    public Color hoverColor;
    private Color baseColor;

    private bool isPlaced;
    private int currentLevel = 0;
    [Header("UI Menu")]
    public GameObject buildMenu;
    public GameObject upgradeSellMenu;
    private GameObject currentMenu;
    public Canvas mainCanvas;
    private List<GameObject> towerType;
    private UpgradeSellMenu currentUpgradeSellMenu;

    [Header("Tower Prefabs")]
    public List<GameObject> cannonTowers;
    public List<GameObject> archerTowers;
    public List<GameObject> magicTowers;
    public GameObject underConstructionPrefab;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        baseColor = myRenderer.color;
        isPlaced = false;
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
        if (currentMenu != null)
        {
            CloseCurrentMenu();
            return;
        }

        if (isPlaced) OpenUpgradeSellMenu();
        else OpenBuildMenu();
    }

    public void OnButtonClicked(string towerToBuild)
    {
        if (isPlaced) return; // If there already exists a turret return

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

            tower = Instantiate(prefabToBuild, transform.position, Quaternion.identity);
            isPlaced = true;
            myRenderer.enabled = false; 
            CloseCurrentMenu();
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
            tower = Instantiate(towerType[currentLevel], transform.position, Quaternion.identity);

            if (currentUpgradeSellMenu != null)
                currentUpgradeSellMenu.UpdateUI(GetNextUpgradeCost()); // Shows next cost

            CloseCurrentMenu();
        }
    }

    public void OnSell()
    {
        int totalCost = 0;
        for (int i = 0; i <= currentLevel; i++)
        {
            totalCost += (int)towerType[i].GetComponent<Tower>().Cost;
        }

        CurrencyManager.Instance.AddGold(Mathf.RoundToInt(totalCost * 0.8f / 10f) * 10);

        Destroy(tower);
        tower = null;
        currentLevel = 0;
        isPlaced = false;

        myRenderer.enabled = false;
        myRenderer.color = baseColor;

        if (currentUnderConstruction != null) 
        { 
            Destroy(currentUnderConstruction); 
        }
        currentUnderConstruction = Instantiate(underConstructionPrefab, transform.position, Quaternion.identity);

        CloseCurrentMenu();
    }


    private void CloseCurrentMenu()
    {
        if (currentMenu == null) return;
        MenuFadeAnimation anim = currentMenu.GetComponent<MenuFadeAnimation>();
        if (anim != null) anim.CloseMenu();
        else Destroy(currentMenu);
        currentMenu = null;
    }

    public void OpenBuildMenu()
    {
        currentMenu = Instantiate(buildMenu, mainCanvas.transform);
        currentMenu.transform.position = transform.position;
        currentMenu.GetComponent<BuildMenu>().SetSlot(this);
    }

    public void OpenUpgradeSellMenu()
    {
        currentMenu = Instantiate(upgradeSellMenu, mainCanvas.transform);
        currentMenu.transform.position = transform.position;
        var script = currentMenu.GetComponent<UpgradeSellMenu>();
        script.SetSlot(this);
        currentUpgradeSellMenu = script;
    }

    public void SetUpgradeSellMenu(UpgradeSellMenu menu) { currentUpgradeSellMenu = menu; }
    public int GetNextUpgradeCost()
    {
        if (towerType == null || currentLevel >= towerType.Count - 1) return 0;
        return (int)towerType[currentLevel + 1].GetComponent<Tower>().Cost;
    }
}