using UnityEngine;
using UnityEngine.EventSystems;



public class PlayerWeaponController : MonoBehaviour
{

    [Header("References")]
    public Player_Combat sword;
    public Player_Bow bow;
    public PlayerInventory inventory;
    public ActiveInventory activeInventory;

    private void Start()
    {
        if (activeInventory == null)
            activeInventory = FindObjectOfType<ActiveInventory>();
    }

    private void Update()
    {
#if !UNITY_ANDROID && !UNITY_IOS
        HandlePCInput();
#endif
    }

    // Gestisce l'input del mouse per attaccare o usare pozioni
    private void HandlePCInput()
    {
        int activeSlot = activeInventory.GetActiveSlot();

        // Click sinistro
        if (Input.GetMouseButtonDown(0))
        {
            // Evita attacco se clicchi sulla UI
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            UseActiveSlot(activeSlot);
        }
    }

    // Input da mobile
    public void OnAttackButton()
    {

        int activeSlot = activeInventory.GetActiveSlot();
        UseActiveSlot(activeSlot);
    }

    private void UseActiveSlot(int activeSlot)
    {
        switch (activeSlot)
        {
            case 0:
                sword.Attack();
                break;

            case 1:
                bow.StartShooting();
                break;

            case 2:
                inventory.UsePotion();
                break;
        }
    }
}
