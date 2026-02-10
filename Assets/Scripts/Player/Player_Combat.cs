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

    public LayerMask chestLayer; // Layer mask to identify chests   

    public Animator anim; // Reference to the Animator component

    public float cooldown = 1.2f; // Cooldown time in seconds

    private float timer; // Timer to track the cooldown

    // Versione senza input, da chiamare da PlayerMovement
    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
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

        foreach (Collider2D enemy in enemies)
        {
            // Se è un nemico base
            if (enemy.TryGetComponent<Enemy_Health>(out Enemy_Health enHealth))
            {
                enHealth.ChangeHealth(-damage);
                enemy.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            }
            // SE È IL BOSS
            else if (enemy.TryGetComponent<BossHealth>(out BossHealth bHealth))
            {
                bHealth.TakeDamage(damage);
                // Se vuoi dare knockback anche al boss, serve uno script BossKnockback!
            }
        }

            // COLPO AL BAULE
            Collider2D[] chests = Physics2D.OverlapCircleAll(attakPoint.position, weaponRange, chestLayer);

        foreach (Collider2D chest in chests)
        {
            chest.GetComponent<Chest>()?.TakeDamage(damage);
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
