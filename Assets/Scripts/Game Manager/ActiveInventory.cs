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

        if (targetIndex >= 0 && targetIndex < transform.childCount)
        {
            ToggleActiveHighLight(targetIndex);
        }
    }

    private void ToggleActiveHighLight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

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
