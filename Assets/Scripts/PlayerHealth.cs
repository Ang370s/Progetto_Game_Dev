using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 6;
    public float currentHealth = 6;

    // Trascina l'oggetto con lo script HealthHeartBar qui nell'Inspector
    public HealthHeartBar heartBar;

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // AGGIORNAMENTO: Chiama il ridisegno dei cuori
        if (heartBar != null)
        {
            heartBar.DrawHearts();
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}