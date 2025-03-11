using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopSlot[] slots;
    public ConsumableItem[] possibleItems;
    public GameObject itemPrefab;
    private InventoryManager inventoryManager;
    public void AddRandomItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ConsumableItem randomItem = possibleItems[Random.Range(0, possibleItems.Length)];

            if (slots[i].transform.childCount == 0) 
            {
                Debug.Log($"Giving {i+1} item");
                GameObject newItem = Instantiate(itemPrefab, slots[i].transform);
                InventoryItem itemSprite = newItem.GetComponent<InventoryItem>();
                TooltipTrigger itemTooltip = newItem.GetComponent<TooltipTrigger>();
                itemSprite.GetItemData(randomItem);
                itemTooltip.GetItemData(randomItem);
            }

            Debug.Log("Shop Full!"); // If no slot is free
        }
    }
    public void DestroyShopItems()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"Trashing {i+1} item");

            if (slots[i].transform.childCount > 0)
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
            Debug.Log($"Clear!");
        }
    }
}