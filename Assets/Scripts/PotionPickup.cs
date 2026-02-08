using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            bool pickedUp = inventory.AddPotion();

            if (pickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}
