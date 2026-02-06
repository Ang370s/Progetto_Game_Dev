using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    
    public int currentHealth;
    public int maxHealth = 3;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        if (isDead)
        {
            return; // Non fare nulla se il nemico è già morto
        }

        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        else if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject); // per ora
        }
    }
}
