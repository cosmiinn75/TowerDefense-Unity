using TMPro;
using UnityEngine;

public class UpgradeSellMenu : MonoBehaviour
{
    public SlotManager currentSlot;
    public TextMeshProUGUI priceText;

    public void SetSlot(SlotManager slot)
    {
        currentSlot = slot;
        currentSlot.SetUpgradeSellMenu(this);
        UpdateInitialPrice();
    }

    public void OnUpgradeClick()
    {
        currentSlot.OnUpgrade();
    }

    public void OnSellClick()
    {
        currentSlot.OnSell();
    }

    public void UpdateUI(int price)
    {
        priceText.text = price.ToString() + "g";
    }


    private void UpdateInitialPrice()
    {
        if (currentSlot != null)
        {
    
            int nextPrice = currentSlot.GetNextUpgradeCost();
            UpdateUI(nextPrice);
        }
    }
}