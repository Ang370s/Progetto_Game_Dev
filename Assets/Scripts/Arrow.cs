using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component of the arrow
    public Vector2 direction = Vector2.right; // Direction in which the arrow will move
    public float lifeSpawn = 2; // Time in seconds after which the arrow will be destroyed (2 seconds)
    public float speed = 10f; // Speed at which the arrow will move

    public LayerMask enemyLayer; // Layer mask to identify enemies
    public LayerMask obstacleLayer; // Layer mask to identify obstacles

    public SpriteRenderer sr; // Reference to the SpriteRenderer component of the arrow
    public Sprite buriedSprite; // Sprite to be used when the arrow is buried in an obstacle

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

        else if((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0) // Check if the collided object is in the obstacle layer
        {
            AttachToTarget(collision.gameObject.transform); // Attach the arrow to the obstacle by calling the AttachToTarget method with the transform of the collided object
        }
    }

    private void AttachToTarget(Transform target)
    {
        sr.sprite = buriedSprite; // Change the sprite of the arrow to the buried sprite

        rb.velocity = Vector2.zero; // Stop the arrow's movement by setting its velocity to zero
        rb.isKinematic = true; // Make the Rigidbody2D kinematic to prevent it from being affected by physics

        transform.SetParent(target); // Set the parent of the arrow to the target, so it will move with the target
    }

}
