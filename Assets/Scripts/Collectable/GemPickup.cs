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
        if (!collision.CompareTag("Player"))
            return;

        if (PlayerStats.Instance == null)
            return;

        PlayerStats.Instance.AddGem(gemType, value);
        Destroy(gameObject);
    }
}