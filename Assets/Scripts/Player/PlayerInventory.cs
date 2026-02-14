using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Potions")]
    public int potionCount = 0;
    public int maxPotions = 5;
    public int healAmount = 2;

    private PlayerHealth playerHealth;
    private SpriteRenderer sr;

    void Start()
    {
        if (PlayerStats.Instance != null)
        {
            potionCount = PlayerStats.Instance.savedPotions;
        }
    }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        sr = GetComponent<SpriteRenderer>();
    }

    public bool AddPotion()
    {
        if (potionCount >= maxPotions)
            return false;

        potionCount++;
        return true;
    }

    public void UsePotion()
    {
        // 1? Se non ho pozioni, esco
        if (potionCount <= 0)
            return;

        // 2? Se ho già vita piena, NON uso la pozione
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
            return;

        // 3? Uso la pozione
        potionCount--;

        playerHealth.ChangeHealth(healAmount);
        StartCoroutine(PotionFeedback());
    }

    IEnumerator PotionFeedback()
    {
        Color original = sr.color;
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        sr.color = original;
    }
}
