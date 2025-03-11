using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public UnityEngine.UI.Image image;
    public ConsumableItem CurrentItem;
    public bool isShopItem = false;

    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isShopItem) return;

        //Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent; // To remember the parent of the item
        transform.SetParent(transform.root); // To make sure the item is not a child of the slot
        transform.SetAsLastSibling(); // To make sure the item is on top of everything
        image.raycastTarget = false; 
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(isShopItem) return;

        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(isShopItem) return;
        
        //Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); 
        image.raycastTarget = true;
    }

    public void GetItemData(ConsumableItem itemData, bool isFromShop)
    {
        CurrentItem = itemData;
        image.sprite = itemData.Sprite;
        isShopItem = isFromShop;
    }

    public void OnPointerClick(PointerEventData eventData) // When the item is clicked, only work for shop items cuz if(isShopItem)
    {
        if (isShopItem) // If the item is from the shop
        {
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            ShopManager shopManager = FindObjectOfType<ShopManager>(); 

            if (inventory.TryBuyItem(CurrentItem)) // If player have enough Doubloons
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Not enough Doubloons!");
            }
        }
    }
}
