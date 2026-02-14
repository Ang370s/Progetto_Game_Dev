using TMPro;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    public PlayerInventory inventory;
    public TextMeshProUGUI potionText;
    public GameObject potionIcon;

    void Start()
    {
        if (inventory == null)
            inventory = FindObjectOfType<PlayerInventory>();
    }

    void Update()
    {
        if (inventory == null) return;

        potionText.text = inventory.potionCount.ToString();
        potionIcon.SetActive(inventory.potionCount > 0);
    }
}
