using UnityEngine;

public class SellMenu : MonoBehaviour
{
    public SlotManager currentSlot;

   public void SetSlot(SlotManager slotManager)
    {
        currentSlot = slotManager;
    }

    public void OnSell()
    {
        currentSlot.OnSell();
    }
}
