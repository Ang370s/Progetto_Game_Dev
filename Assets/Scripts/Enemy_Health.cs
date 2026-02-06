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
            return; // Non fare nulla se il nemico è già morto
        }

        if (isInvulnerable && amount < 0)
        {
            return; // Non fare nulla se il nemico è invulnerabile e si sta cercando di infliggere danno
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
        isDead = true;
        Destroy(gameObject); // per ora
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
