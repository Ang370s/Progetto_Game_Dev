using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private InputSystem_Actions playerControls;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        // Forza l'evidenziazione del primo slot all'avvio
        ToggleActiveHighLight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        // numValue è il tasto premuto (1, 2, 3...). Sottraiamo 1 per l'indice (0, 1, 2...)
        int targetIndex = numValue - 1;

        // Controllo di sicurezza: se l'indice esiste nella lista dei figli
        if (targetIndex >= 0 && targetIndex < transform.childCount)
        {
            ToggleActiveHighLight(targetIndex);
        }
    }

    private void ToggleActiveHighLight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }

    public int GetActiveSlot()
    {
        return activeSlotIndexNum;
    }

}
