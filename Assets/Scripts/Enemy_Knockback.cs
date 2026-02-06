using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Enemy_Movement enemy_Movement; // Reference to the Enemy_Movement script

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        enemy_Movement = GetComponent<Enemy_Movement>(); // Get the Enemy_Movement script
    }   

    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.Knockback); // Change the enemy's state to Knockback
        StartCoroutine(StunTimer(knockbackTime, stunTime)); // Start the stun timer coroutine
        Vector2 direction = (transform.position - playerTransform.position).normalized; // Calcola la direzione del knockback
        rb.linearVelocity = direction * knockbackForce; // Applica la forza di knockback al Rigidbody2D
        Debug.Log("Kncockback applied");
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime); // Wait for the knockback time to elapse
        rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
        yield return new WaitForSeconds(stunTime); // Wait for the stun time to elapse
        enemy_Movement.ChangeState(EnemyState.Idle); // Change the enemy's state back to Idle
    }
}
