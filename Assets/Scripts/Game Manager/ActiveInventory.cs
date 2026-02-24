using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private InputSystem_Actions playerControls;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    // This method is for testing purposes only, to check if the input system is working correctly.
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Debug.Log("CLICK SCREEN");
    }

    private void Start()
    {

        Debug.Log("ActiveInventory START");

        playerControls.Inventory.Keyboard.performed += OnInventoryKeyPressed;
        ToggleActiveHighLight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Inventory.Keyboard.performed -= OnInventoryKeyPressed;
        playerControls.Disable();
    }

    private void OnInventoryKeyPressed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void ToggleActiveSlot(int numValue)
    {
        int targetIndex = numValue - 1;

        SelectSlot(targetIndex);
    }

    public void SelectSlot(int index)
    {
        Debug.Log("Slot premuto: " + index);

        if (index >= 0 && index < transform.childCount)
        {
            ToggleActiveHighLight(index);
        }
    }

    private void ToggleActiveHighLight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        // Disattiva tutti i highlight prima di attivare quello selezionato
        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }

    public int GetActiveSlot()
    {
        return activeSlotIndexNum;
    }
}
