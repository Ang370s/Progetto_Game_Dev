using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed = 2.5f; // Speed of the enemy
    public float attackRange = 1.2f; // Range at which the enemy can attack
    public float attackCooldown = 1.0f; // Cooldown time between attacks in seconds
    public float PlayerDetectRange = 3.0f; // Range at which the enemy can detect the player
    public Transform detectionPoint; // Point from which to detect the player
    public LayerMask playerLayer; // Layer mask to identify the player

    private float attackCooldownTimer; // Timer to track attack cooldown
    private int facingDirection = 1; // 1 for right, -1 for left
    private EnemyState enemyState; // Current state of the enemy

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Transform player; // Reference to the player's Transform
    private Animator anim; // Reference to the Animator component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>(); // Get the Animator component
        ChangeState(EnemyState.Idle); // Start in Idle state
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer(); // Check if the player is within detection range

        // Update the attack cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame
        }

        // Handle behavior based on the current state
        if (enemyState == EnemyState.Chasing)
        {
            Chase(); // Call the Chase method to handle chasing behavior
        }

        // Handle Attacking state (if needed)
        else if (enemyState == EnemyState.Attacking)
        {
            // Do Attacking behavior here
            rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
        }
    }

    // Method to handle chasing behavior
    void Chase()
    {
        // Flip the enemy to face the player if necessary
        if (player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip(); // Call the Flip method to change the facing direction
        }
        Vector2 direction = (player.position - transform.position).normalized; // Calculate the direction towards the player
        rb.linearVelocity = direction * speed; // Move towards the player at a speed of 3 units per second
    }

    // Method to flip the enemy's facing direction
    void Flip()
    {
        facingDirection *= -1; // Reverse the facing direction
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); // Flip the enemy's scale on the x-axis
    }

    // Method to check for the player within detection range
    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, PlayerDetectRange, playerLayer);

        // If the player is detected within range
        if (hits.Length > 0)
        {
            player = hits[0].transform; // Get the player's Transform

            // Check if the player is within attack range and if the attack cooldown has elapsed
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown; // Reset the attack cooldown timer
                ChangeState(EnemyState.Attacking); // Change to Attacking state
            }

            // If the player is outside attack range, switch to Chasing state
            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing); // Change to Chasing state
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
            ChangeState(EnemyState.Idle);
        }
    }

    // Method to change the enemy's state and update animations accordingly
    void ChangeState(EnemyState newState)
    {
        // Exit the current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        // Update the current state
        enemyState = newState;

        // Update the new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }

    // Draw gizmos in the editor to visualize detection and attack ranges
    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the editor
        if (detectionPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectionPoint.position, PlayerDetectRange);
        }
        // Draw the attack range in the editor
        if (detectionPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionPoint.position, attackRange);
        }
    }
}

// Enum to represent the different states of the enemy
public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}