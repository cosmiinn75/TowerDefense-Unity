using UnityEngine;

public class ChooseMagicMenu : MonoBehaviour
{
    public SlotManager currentSlot;


    public void SetSlot(SlotManager slot)
    {
        currentSlot = slot;
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
