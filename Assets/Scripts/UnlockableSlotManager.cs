using UnityEngine;

public class UnlockableSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] GameObject unlockSlotMenu;

    private SpriteRenderer myRenderer;
    public Color hoverColor;
    private Color baseColor;

    private GameObject currentMenu;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        baseColor = myRenderer.color;
    }
    private void OnMouseEnter()
    {      
            if (myRenderer!= null) myRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (myRenderer != null) myRenderer.color = baseColor;
    }

    private void OnMouseDown()
    {
        if (currentMenu == null) {

            OpenUnlockableSlotMenu();
        }
        else
        {
            CloseCurrentMenu();
        }
    }

    void OpenUnlockableSlotMenu() {
        currentMenu = Instantiate(unlockSlotMenu, mainCanvas.transform);
        currentMenu.transform.position = transform.position;
      ////  var script = currentMenu.GetComponent<UnlockableSlotMenu>();
      //  if (script != null) {
      //      script.SetSlot(this);
      //  }

    }
    private void CloseCurrentMenu()
    {
        if (currentMenu == null) return;
        MenuFadeAnimation anim = currentMenu.GetComponent<MenuFadeAnimation>();
        if (anim != null) anim.CloseMenu();
        else Destroy(currentMenu);
        currentMenu = null;

    }
}
