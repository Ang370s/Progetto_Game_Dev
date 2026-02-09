using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int killCount = 0;
    public int gemCount = 0;

    [Header("Potion Drop")]
    public int killsForPotion = 3;
    public GameObject potionPrefab;

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

    public void AddGem(int amount = 1)
    {
        gemCount += amount;
    }
}
