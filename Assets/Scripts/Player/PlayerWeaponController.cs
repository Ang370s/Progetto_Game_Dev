using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow
}

public class PlayerWeaponController : MonoBehaviour
{
    public WeaponType currentWeapon = WeaponType.Sword;

    public Player_Combat sword;
    public Player_Bow bow;
    public PlayerInventory inventory;
    public ActiveInventory activeInventory;

    private void Start()
    {
        EquipSword(); // default
    }

    private void Update()
    {
        int activeSlot = activeInventory.GetActiveSlot();

        // Switch weapon SOLO se slot 0 o 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipSword();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipBow();

        // Click sinistro
        if (Input.GetMouseButtonDown(0))
        {
            // SLOT 0 -> SPADA
            if (activeSlot == 0)
            {
                sword.Attack();
            }
            // SLOT 1 -> ARCO
            else if (activeSlot == 1)
            {
                bow.StartShooting();
            }
            // SLOT 2 -> POZIONE
            else if (activeSlot == 2)
            {
                inventory.UsePotion();
            }
        }
    }


    /*private void Update()
    {
        // Switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipSword();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipBow();

        if (Input.GetKey(KeyCode.Alpha3) && Input.GetMouseButtonDown(0))
            inventory.UsePotion();

        // Attack input (unificato)
        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon == WeaponType.Sword && currentWeapon == WeaponType.Sword)
                sword.Attack();

            if (currentWeapon == WeaponType.Bow && currentWeapon == WeaponType.Bow)
                bow.StartShooting();
        }
    }*/

    void EquipSword()
    {
        currentWeapon = WeaponType.Sword;
        sword.enabled = true;
        bow.enabled = false;
    }

    void EquipBow()
    {
        currentWeapon = WeaponType.Bow;
        sword.enabled = false;
        bow.enabled = true;
    }
}
