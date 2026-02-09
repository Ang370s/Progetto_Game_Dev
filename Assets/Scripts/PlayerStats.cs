using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance; // PROVA

    public bool hasKey = false;

    public int killCount = 0;

    [Header("Gems")]
    public int diamondCount = 0;
    public int emeraldCount = 0;
    public int goldCount = 0;

    [Header("Gem UI")]
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI emeraldText;
    public TextMeshProUGUI goldText;

    [Header("Potion Drop")]
    public int killsForPotion = 3;
    public GameObject potionPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddKill(Vector3 enemyPosition)
    {
        killCount++;

        // Ogni 3 kill
        if (killCount % killsForPotion == 0)
        {
            DropPotion(enemyPosition);
        }
    }

    void DropPotion(Vector3 position)
    {
        Instantiate(potionPrefab, position, Quaternion.identity);
    }

    public void AddGem(GemType type, int amount = 1)
    {
        switch (type)
        {
            case GemType.Diamond:
                diamondCount += amount;
                if (diamondText != null) diamondText.text = diamondCount.ToString();
                break;

            case GemType.Emerald:
                emeraldCount += amount;
                if (emeraldText != null) emeraldText.text = emeraldCount.ToString();
                break;

            case GemType.Gold:
                goldCount += amount;
                if (goldText != null) goldText.text = goldCount.ToString();
                break;
        }
    }

    public void GetKey()
    {
        hasKey = true;
    }
}