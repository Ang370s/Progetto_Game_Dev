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

    [Header("Statistics")]
    public static int chestOpened = 0;
    private static int guaranteedAfter = 20; // Garantisce una chiave dopo 20 chest aperti senza chiave trovata

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

        chestOpened++;

        Debug.Log("Chest opened! Total: " + chestOpened);

        //PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (!keyHasDropped)
        {
            // 5% di possibilità di droppare la chiave, o garantire una chiave dopo un certo numero di chest aperti senza chiave
            if (Random.value < 0.05f || chestOpened > guaranteedAfter)
            {
                Instantiate(keyPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                keyHasDropped = true;
                return;
            }
        }

        // Drop oggetti comuni
        DropRandomObject();
        SFXManager.Instance.PlaySFX(SFXManager.Instance.chestBreak);

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

    public static void ResetChestStats()
    {
        chestOpened = 0;
        keyHasDropped = false;
    }
}