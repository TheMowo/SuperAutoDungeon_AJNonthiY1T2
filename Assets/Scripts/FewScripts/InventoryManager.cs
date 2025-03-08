using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots; // Reference to all inventory slots
    public GameObject inventoryItemPrefab; // The prefab for the item to be added

    // Called when the button is pressed
    public void AddItem()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0) // Check for the first empty slot
            {
                // Instantiate a new item and set it in the slot
                GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
                return; // Exit after placing the item
            }
        }

        Debug.Log("No empty slots available!"); // If no slot is free
    }
}
