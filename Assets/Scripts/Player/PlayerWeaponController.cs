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

        // Il gamepad è supportato su tutte le piattaforme, quindi lo gestiamo sempre
        HandleGamepadInput();
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

    private void HandleGamepadInput()
    {

        // Tasto A (Attacco)
        if (Input.GetKeyDown(KeyCode.JoystickButton0)) // A
        {
            OnAttackButton();
        }

        // Dorsali (Cambio Arma)
        if (Input.GetKeyDown(KeyCode.JoystickButton4)) // L1
        {
            ChangeWeapon(-1);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) // R1
        {
            ChangeWeapon(1);
        }
    }

    // Una piccola funzione di supporto per cambiare slot
    private void ChangeWeapon(int direction)
    {
        int current = activeInventory.GetActiveSlot();
        int next = (current + direction + 3) % 3; // Il +3 serve a non andare in negativo
        activeInventory.SelectSlot(next); // Assicurati che il nome della funzione in ActiveInventory sia corretto
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
