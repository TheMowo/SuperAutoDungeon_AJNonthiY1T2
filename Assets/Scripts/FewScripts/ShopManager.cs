using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopSlot[] slots;
    public TMPro.TextMeshProUGUI[] priceTexts;
    public ConsumableItem[] possibleItems;
    public GameObject itemPrefab;
    
    public GameObject[] shopOpenState;
    public GameObject[] shopCloseState;
    public GameObject nextStageButton;
    [HideInInspector] public GameObject inventoryManagerObject;
    [HideInInspector] public InventoryManager inventoryManager;

    private void Awake()
    {
        AddRandomItems(8);
        inventoryManagerObject = GameObject.FindGameObjectWithTag("InventoryManager");
        inventoryManager = inventoryManagerObject.GetComponent<InventoryManager>();
    }
    
    public void AddRandomItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ConsumableItem randomItem = possibleItems[Random.Range(0, possibleItems.Length)];

            if (slots[i].transform.childCount == 0) 
            {
                GameObject newItem = Instantiate(itemPrefab, slots[i].transform);
                InventoryItem itemScript = newItem.GetComponent<InventoryItem>(); // This GetComponent get the item's stats from scriptableobject
                TooltipTrigger itemTooltip = newItem.GetComponent<TooltipTrigger>(); // This GetComponent get the tooltip script
                
                itemScript.GetItemData(randomItem, true);
                itemTooltip.GetItemData(randomItem);

                priceTexts[i].text = $"{randomItem.price} D";
            }
        }
        Debug.Log($"Restocking {count} item; restocked!");
    }
    public void DestroyShopItems()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
        }
        Debug.Log($"Trashing all items; trashed!");
    }

    public void GenerateShopItem()
    {
        DestroyShopItems();
        Debug.Log("Shop Items Cleared");
        Delay.Run(0.01f, () => Debug.Log("Shop Items Created"));
        Delay.Run(0.01f, () => AddRandomItems(8));
    }
    public void RerollShopItem()
    {
        if(inventoryManager.playerCurrency < 5)
        {
            Debug.Log("Not Enough Currency!");
        }
        else
        {
            inventoryManager.playerCurrency -= 5;
            inventoryManager.UpdateCurrencyUI();
            DestroyShopItems();
            Debug.Log("Shop Items Cleared");
            Delay.Run(0.01f, () => Debug.Log("Shop Items Created"));
            Delay.Run(0.01f, () => AddRandomItems(8));
        }
    }

    public void OpenShopOnWin()
    {
        Debug.Log("OpenShopOnWin");
        AddRandomItems(8);
        foreach (var shopO in shopOpenState)
        {
            shopO.SetActive(true);
        }
        foreach (var shopC in shopCloseState)
        {
            shopC.SetActive(false);
        }
        nextStageButton.SetActive(true);
    }
}