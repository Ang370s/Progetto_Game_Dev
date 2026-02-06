using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Transform attakPoint; // Point from which the attack is initiated
    public float weaponRange = 1.0f; // Range of the weapon attack
    public float knockbackForce = 5f; // Force applied for knockback
    public float knockbackTime = 0.15f; // Time for which the player is knocked back
    public float stunTime = 0.3f; // Time for which the player is stunned after being hit
    public LayerMask enemyLayer; // Layer mask to identify enemies
    public int damage = 1; // Damage dealt by the attack

    public Animator anim; // Reference to the Animator component

    public float cooldown = 0.8f; // Cooldown time in seconds

    private float timer; // Timer to track the cooldown

    // Update is called once per frame
    private void Update()
    {
        // Decrease the timer if it's greater than 0
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    // Method to handle the attack action
    public void Attack()
    {
        // Check if the timer has reached 0 or less, allowing the player to attack
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
    }

    // Method to reset the attacking state, called at the end of the attack animation
    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attakPoint.position, weaponRange, enemyLayer);

        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies) 
            {
                enemy.GetComponent<Enemy_Health>().ChangeHealth(-damage);
                enemy.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            }
            

            //enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);
            //enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attakPoint == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attakPoint.position, weaponRange);
    }
}
