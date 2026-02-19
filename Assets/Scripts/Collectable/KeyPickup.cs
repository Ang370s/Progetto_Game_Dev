using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (PlayerStats.Instance == null)
            return;

        PlayerStats.Instance.GetKey();
        SFXManager.Instance.PlaySFX(SFXManager.Instance.gem);
        Destroy(gameObject);
    }
}