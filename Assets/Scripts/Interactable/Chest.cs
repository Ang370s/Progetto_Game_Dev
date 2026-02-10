using UnityEngine;

public class Chest : MonoBehaviour
{
    public int health = 2;

    [Header("Drop")]
    public GameObject goldPrefab;
    public GameObject emeraldPrefab;
    public GameObject diamondPrefab;
    public GameObject keyPrefab;
    public GameObject potionPrefab;

    private bool isDestroyed = false;
    private static bool keyHasDropped = false;

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

        //PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (!keyHasDropped)
        {
            // 05% possibilità di chiave
            if (Random.value < 0.05f)
            {
                Instantiate(keyPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                keyHasDropped = true;
                return;
            }
        }

        // Drop oggetti comuni
        DropRandomObject();

        Destroy(gameObject);
    }

    void DropRandomObject()
    {
        float roll = Random.value;

        if (roll < 0.5f)
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        else if (roll < 0.8f)
            Instantiate(emeraldPrefab, transform.position, Quaternion.identity);
        else if (roll < 0.95f)
            Instantiate(diamondPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(potionPrefab, transform.position, Quaternion.identity);
    }
}