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

    private void Start()
    {
        EquipSword(); // default
    }


    private void Update()
    {
        // Switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipSword();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipBow();

        // Attack input (unificato)
        if (Input.GetMouseButtonDown(0))
        {
            if (currentWeapon == WeaponType.Sword && currentWeapon == WeaponType.Sword)
                sword.Attack();

            if (currentWeapon == WeaponType.Bow && currentWeapon == WeaponType.Bow)
                bow.StartShooting();
        }
    }

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
