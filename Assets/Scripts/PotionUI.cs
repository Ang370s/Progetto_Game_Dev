using TMPro;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    public PlayerInventory inventory;
    public TextMeshProUGUI potionText;
    public GameObject potionIcon;

    void Update()
    {
        potionText.text = inventory.potionCount.ToString();
        potionIcon.SetActive(inventory.potionCount > 0);
    }
}
