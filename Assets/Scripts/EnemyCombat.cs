using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1; // Damage dealt to the player
    public Transform attackPoint; // Point from which the attack is made
    public float weaponRange = 1.2f; // Range of the weapon
    public float knockbackForce = 5.0f; // Force applied to the player when hit
    public float stunTime = 0.3f; // Time the player is stunned after being hit
    public LayerMask playerLayer; // Layer mask to identify the player

    // Method to perform an attack
    public void Attack()
    {
        // Detect players within the attack range
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        // If a player is hit, deal damage
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage); // Deal damage to the first player hit
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunTime); // Apply knockback to the player
        }
    }
}
