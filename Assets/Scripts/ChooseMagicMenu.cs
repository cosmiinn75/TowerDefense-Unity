using TMPro;
using UnityEngine;
using UnityEngine.UI; // Obligatoriu pentru a folosi componenta clasică Text
// foloseste "using TMPro;" în loc dacă ai texte de tip TextMeshPro

public class ChooseMagicMenu : MonoBehaviour
{
    public SlotManager currentSlot;

    [Header("UI Cost Texts")]
    public TextMeshProUGUI iceCostText;      
    public TextMeshProUGUI lightningCostText; 
    public TextMeshProUGUI poisonCostText;  

    public void SetSlot(SlotManager slot)
    {
        currentSlot = slot;
    }

    public void UpdateCostTexts(int cost)
    {
        if (cost != 0)
        {
            if (iceCostText != null) iceCostText.text = cost.ToString() + "g";
            if (lightningCostText != null) lightningCostText.text = cost.ToString() + "g";
            if (poisonCostText != null) poisonCostText.text = cost.ToString() + "g";
        } else
        {
            if (iceCostText != null) iceCostText.text = "free";
            if (lightningCostText != null) lightningCostText.text = "free";
            if (poisonCostText != null) poisonCostText.text = "free";
        }
    }

    public void OnSellClick()
    {
        currentSlot.OnSell();
    }

    public void OnIce()
    {
        currentSlot.OnSelectElement("Ice");
    }

    public void OnLightning()
    {
        currentSlot.OnSelectElement("Lightning");
    }

    public void OnPoison()
    {
        currentSlot.OnSelectElement("Poison");
    }
}