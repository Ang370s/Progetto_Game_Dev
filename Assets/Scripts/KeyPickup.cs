using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerStats.Instance == null)
            return;

        PlayerStats.Instance.GetKey();
        Destroy(gameObject);
    }

}