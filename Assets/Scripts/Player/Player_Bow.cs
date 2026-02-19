using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform launchPoint; // Point from which the arrow will be launched
    public GameObject arrowPrefab; // Prefab of the arrow to be instantiated when shooting
    public Animator anim; // Reference to the Animator component

    private Vector2 aimDirection = Vector2.right; // Direction in which the player is aiming

    public float shootCooldown = 1.2f; // Cooldown time between shots in seconds
    private float shootTimer; // Timer to track the cooldown between shots

    // Versione senza input, da chiamare da PlayerWeaponController
    void Update()
    {
        shootTimer -= Time.deltaTime;
        HandleAiming();
    }

    public void StartShooting()
    {
        if (shootTimer > 0)
            return;

        anim.SetBool("isShooting", true);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.bow);
    }

    // Method to handle the shooting action
    public void Shoot()
    {
        // Check if the shoot timer has reached 0 or less, allowing the player to shoot
        if (shootTimer > 0)
            return;

        // Instantiate a new arrow at the launch point with no rotation and get the Arrow component from it
        Arrow arrow = Instantiate(
            arrowPrefab,
            launchPoint.position,
            Quaternion.identity
        ).GetComponent<Arrow>();

        arrow.direction = aimDirection;
        shootTimer = shootCooldown;
    }

    public void FinishShooting()
    {
        anim.SetBool("isShooting", false);
    }


    public void HandleAiming()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        float vertical = Input.GetAxisRaw("Vertical"); // Get vertical input

        if (horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized; // Update the aim direction based on input and normalize it
        }
    }

}
