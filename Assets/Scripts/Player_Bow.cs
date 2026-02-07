using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform launchPoint; // Point from which the arrow will be launched
    public GameObject arrowPrefab; // Prefab of the arrow to be instantiated when shooting

    private Vector2 aimDirection = Vector2.right; // Direction in which the player is aiming

    public float shootCooldown = 0.5f; // Cooldown time between shots in seconds
    private float shootTimer; // Timer to track the cooldown between shots

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime; // Decrease the shoot timer by the time elapsed since the last frame

        HandleAiming(); // Call the method to handle aiming input

        // Check if the right mouse button is pressed and the shoot timer has reached 0 or less, allowing the player to shoot
        if (Input.GetMouseButtonDown(1) && shootTimer <= 0)
        {
            Shoot(); // Call the Shoot method to check for shooting input
        }
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

    public void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>(); // Instantiate the arrow prefab at the launch point and get the Arrow component
        arrow.direction = aimDirection; // Set the direction of the arrow to the current aim direction
        shootTimer = shootCooldown; // Reset the shoot timer after shooting
    }

}
