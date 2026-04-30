using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    private SlotManager currentSlot;
   private string cannonTowers = "cannonTowers";
    private string archerTowers = "archerTowers";
    private string magicTowers = "magicTowers";
    public void SetSlot(SlotManager slot)
    {
        currentSlot = slot;
    }

    public void OnArcherTowerButton()
    {
        if (currentSlot != null)
        {
            currentSlot.OnButtonClicked(archerTowers);
        }
    }
    public void OnCannonTowerButton()
    {
        if (currentSlot != null)
        {
            currentSlot.OnButtonClicked(cannonTowers);
        }
    }
    public void OnMagicTowerButton()
    {
        if (currentSlot != null)
        {
            currentSlot.OnButtonClicked(magicTowers);
        }
    }
}
