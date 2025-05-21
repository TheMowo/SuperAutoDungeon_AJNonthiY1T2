using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots;
    public InventorySlot trashSlot;
    public ConsumableItem[] possibleItems;
    public GameObject itemPrefab;

    public int playerCurrency = 100;
    public TMP_Text currencyText;

    void Start()
    {
        if (CurrencySaveSystem.Instance != null)
        {
            CurrencySaveSystem.Instance.FindCurrency(this);
        }
        Delay.Run(0.01f, () => UpdateCurrencyUI());
    }


    public void UpdateCurrencyUI()
    {
        currencyText.text = $"Doubloon: {playerCurrency}";
    }

    [System.Obsolete]
    public bool TryBuyItem(ConsumableItem item)
    {
        if (playerCurrency >= item.price)
        {
            playerCurrency -= item.price;
            AddItemToInventory(item); // Add the item to inventory
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    [System.Obsolete]
    public void AddItemToInventory(ConsumableItem item) // Method for "TryBuyItem", adding the item into inventory bought from shop
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                InventoryItem itemScript = newItem.GetComponent<InventoryItem>(); // This GetComponent get the item's stats from scriptableobject
                TooltipTrigger itemTooltip = newItem.GetComponent<TooltipTrigger>(); // This GetComponent get the tooltip script
                itemScript.GetItemData(item, false); // Mark as not from shop
                itemTooltip.GetItemData(item);
                return;
            }
        }
        Debug.Log("Inventory Full!");
    }
    private void DestroyTrashItem() // For the trash button, removes the item from the trash slot.
    {
        Debug.Log($"Trashing item");

        if (trashSlot.transform.childCount > 0)
        {
            Destroy(trashSlot.transform.GetChild(0).gameObject);
        }
        Debug.Log($"Clear!");
    }

    [System.Obsolete]
    public void AddRandomItem() // For the cheat button, gives random item into inventory.
    {
        Debug.Log("Adding random item");
        ConsumableItem randomItem = possibleItems[Random.Range(0, possibleItems.Length)];

        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0) // This line check for the first empty slot
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                InventoryItem itemScript = newItem.GetComponent<InventoryItem>();
                TooltipTrigger itemTooltip = newItem.GetComponent<TooltipTrigger>();
                itemScript.GetItemData(randomItem, false);
                itemTooltip.GetItemData(randomItem);
                return;
            }
        }

        Debug.Log("Inventory Full!"); // If no slot is free
    }

    public CurrencySaveData GetDataSave()
    {
        return new CurrencySaveData
        {
            doubloons = this.playerCurrency,
        };
    }

    public void LoadFromSaveData(CurrencySaveData data)
    {
        this.playerCurrency = data.doubloons;
    }
}
