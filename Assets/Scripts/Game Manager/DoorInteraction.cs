using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    private bool playerNearby = false;

    void Update()
    {
        if (!playerNearby) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryEnterDoor();
        }
    }

    void TryEnterDoor()
    {
        if (PlayerStats.Instance == null) return;

        if (PlayerStats.Instance.hasKey)
        {
            PlayerHealth hp = FindObjectOfType<PlayerHealth>();
            PlayerInventory inv = FindObjectOfType<PlayerInventory>();

            if (hp != null)
                PlayerStats.Instance.savedHealth = hp.currentHealth;

            if (inv != null)
                PlayerStats.Instance.savedPotions = inv.potionCount;

            Debug.Log("Entrando nella BossFightScene...");
            SceneManager.LoadScene("BossFightScene");
        }
        else
        {
            Debug.Log("Non hai la chiave!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
