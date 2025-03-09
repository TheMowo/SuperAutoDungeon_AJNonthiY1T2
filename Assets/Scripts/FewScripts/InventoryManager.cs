using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots;
    public ConsumableItem[] possibleItems;
    public GameObject itemPrefab;
    public void AddRandomItem()
    {
        Debug.Log("Test");
        ConsumableItem randomItem = possibleItems[Random.Range(0, possibleItems.Length)];

        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0) // This line check for the first empty slot
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                InventoryItem itemSprite = newItem.GetComponent<InventoryItem>();
                TooltipTrigger itemTooltip = newItem.GetComponent<TooltipTrigger>();
                itemSprite.GetItemData(randomItem);
                itemTooltip.GetItemData(randomItem);
                return;
            }
        }

        Debug.Log("Inventory Full!"); // If no slot is free
    }
}
