using UnityEngine;

public class Chest : MonoBehaviour
{
    public int health = 2;

    [Header("Drop")]
    public GameObject goldPrefab;
    public GameObject emeraldPrefab;
    public GameObject diamondPrefab;
    public GameObject keyPrefab;

    private bool isDestroyed = false;

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        health -= damage;

        if (health <= 0)
        {
            BreakChest();
        }
    }

    void BreakChest()
    {
        isDestroyed = true;

        PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (stats != null && !stats.hasKey)
        {
            // 25% possibilità di chiave
            if (Random.value < 0.25f)
            {
                Instantiate(keyPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
            }
        }

        // Drop gemma
        DropRandomGem();

        Destroy(gameObject);
    }

    void DropRandomGem()
    {
        float roll = Random.value;

        if (roll < 0.6f)
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        else if (roll < 0.9f)
            Instantiate(emeraldPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(diamondPrefab, transform.position, Quaternion.identity);
    }
}