using System.Collections;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public float invulnerabilityTime = 0.3f;

    private bool isInvulnerable = false;
    private bool isDead = false;

    private SpriteRenderer sr;

    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead)
        {
            return; // Non fare nulla se il nemico � gi� morto
        }

        if (isInvulnerable && amount < 0)
        {
            return; // Non fare nulla se il nemico � invulnerabile e si sta cercando di infliggere danno
        }

        currentHealth += amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // ---- KILL COUNT + DROP POZIONE ----
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        if (stats != null)
        {
            stats.AddKill(transform.position);
        }

        // ---- ANIMAZIONE ----
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");

        // ---- BLOCCA LOGICA ----
        Enemy_Movement movement = GetComponent<Enemy_Movement>();
        if (movement != null) movement.enabled = false;

        Enemy_Knockback knockback = GetComponent<Enemy_Knockback>();
        if (knockback != null) knockback.enabled = false;

        // ---- BLOCCA FISICA ----
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // ---- DISABILITA COLLIDER ----
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(invulnerabilityTime);
        sr.color = Color.white;
        isInvulnerable = false;
    }
}
