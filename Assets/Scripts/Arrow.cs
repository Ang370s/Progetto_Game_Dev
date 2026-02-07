using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component of the arrow
    public Vector2 direction = Vector2.right; // Direction in which the arrow will move
    public float lifeSpawn = 2; // Time in seconds after which the arrow will be destroyed (2 seconds)
    public float speed = 10f; // Speed at which the arrow will move

    public LayerMask enemyLayer; // Layer mask to identify enemies

    public int damage = 1; // Damage dealt by the arrow
    public float knockbackForce = 5f; // Force applied for knockback
    public float knockbackTime = 0.15f; // Duration of the knockback effect
    public float stunTime = 0.3f; // Duration of the stun effect after knockback

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = direction * speed; // Set the velocity of the arrow based on the direction and speed
        RotateArrow(); // Call the method to rotate the arrow based on its direction
        Destroy(gameObject, lifeSpawn); // Schedule the destruction of the arrow after lifeSpawn seconds
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in degrees based on the direction vector
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotate the arrow to face the direction of movement
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0) // Check if the collided object is in the enemy layer
        {
            collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-damage); // Inflict damage to the enemy by calling the ChangeHealth method with a negative value
            collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime); // Apply knockback to the enemy by calling the Knockback method with appropriate parameters
        }
    }

}
