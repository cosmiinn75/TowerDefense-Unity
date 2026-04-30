using System;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int currentGold = 100;
    public TextMeshProUGUI goldText;
    private void Awake()
    {
            Instance = this;
    }
    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(float amount)
    {
        int finalAmount = Mathf.RoundToInt(amount / 10f) * 10;
        currentGold += finalAmount;
        UpdateUI();
    }

    public bool TrySpendGold(int amount)
    {
        if(amount > currentGold)
        {
            return false;
        }
        else
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
    }

    public void UpdateUI()
    {
        Debug.Log(currentGold);
        goldText.text = "Gold: " + currentGold.ToString();
    }
}
