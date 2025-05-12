using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class ShopSlot : MonoBehaviour
{
    private TMPro.TextMeshProUGUI priceText;

    public ShopSaveData GetDataSave()
    {
        if (transform.childCount != 0)
        {
            return new ShopSaveData
            {
                SlotIndex = this.transform.GetSiblingIndex(),
                image = this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite,
                item = this.transform.GetChild(0).gameObject.GetComponent<InventoryItem>().CurrentItem
            };
        }
        else
        {
            return new ShopSaveData
            {
                SlotIndex = this.transform.GetSiblingIndex()
            };
        }
    }

    public void LoadFromSaveData(ShopSaveData data)
    {
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = data.image;
        this.transform.GetChild(0).gameObject.GetComponent<InventoryItem>().CurrentItem = data.item;
        this.transform.GetChild(0).gameObject.GetComponent<TooltipTrigger>().currentItem = data.item;
        this.transform.GetChild(0).gameObject.GetComponent<InventoryItem>().isShopItem = true;
        this.transform.GetChild(0).gameObject.GetComponent<TooltipTrigger>().tooltipSystem = FindAnyObjectByType<TooltipSystem>();
    }
}
