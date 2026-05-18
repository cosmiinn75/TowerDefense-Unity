using UnityEngine;

public class UnlockMenu : MonoBehaviour
{

    public SlotManager currentSlot;


    public void SetSlot(SlotManager slot) {
        currentSlot = slot;
    }

    public void OnUnlockMenu() {

        currentSlot.OnUnlock();
    
    }
}
