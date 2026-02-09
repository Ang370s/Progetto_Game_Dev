using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        if (stats != null)
        {
            stats.AddGem(value);
            Destroy(gameObject);
        }
    }
}