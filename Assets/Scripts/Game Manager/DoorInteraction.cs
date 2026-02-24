using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    private bool playerNearby = false;

    [Header("UI Mobile")]
    public GameObject mobileDoorButton;

    void Start()
    {
        // Cerchiamo il bottone nella scena tramite il Tag
        mobileDoorButton = GameObject.FindGameObjectWithTag("DoorButton");

        if (mobileDoorButton != null)
        {
            // Colleghiamo via codice la funzione al click del bottone
            // così non devi farlo a mano nell'Inspector del Prefab!
            Button btn = mobileDoorButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners(); // Pulizia preventiva
                btn.onClick.AddListener(TryEnterDoor);
            }

            mobileDoorButton.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerNearby) return;

        // SE SIAMO SU PC (cioè NON siamo su Android e NON su iOS)
#if !UNITY_ANDROID && !UNITY_IOS
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryEnterDoor();
        }
#endif
    }

    public void TryEnterDoor()
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
            SFXManager.Instance.PlaySFX(SFXManager.Instance.door);
            SceneManager.LoadScene("BossFightScene");
        }
        else
        {
            UIMessage.Instance.ShowMessage("Devi prima trovare la chiave!");
            Debug.Log("Non hai la chiave!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

#if UNITY_ANDROID || UNITY_IOS
            // Accesso diretto e velocissimo tramite il Singleton
            if (MobileUIManager.Instance != null && MobileUIManager.Instance.doorButton != null)
            {
                mobileDoorButton = MobileUIManager.Instance.doorButton;

                // Colleghiamo l'evento se non lo abbiamo ancora fatto
                Button btn = mobileDoorButton.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(TryEnterDoor);

                mobileDoorButton.SetActive(true);
            }
#endif
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            // Nascondiamo il bottone quando il player si allontana (su tutte le piattaforme per sicurezza)
            if (mobileDoorButton != null)
            {
                mobileDoorButton.SetActive(false);
            }
        }
    }
}
