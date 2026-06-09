using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    private SlotManager currentSlot;
    public void SetSlot(SlotManager slot)
    {
        currentSlot = slot;
    }

    public void BuyTower(string tower)
    {
        if(currentSlot != null)
        {
            currentSlot.OnButtonClicked(tower);
        }
    }
}
