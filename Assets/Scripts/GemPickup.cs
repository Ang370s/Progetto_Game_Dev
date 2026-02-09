using UnityEngine;

public enum GemType
{
    Diamond,
    Emerald,
    Gold
}

public class GemPickup : MonoBehaviour
{
    public GemType gemType;
    public int value = 1;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        if (stats != null)
        {
            stats.AddGem(gemType, value);
            Destroy(gameObject);
        }
    }
}