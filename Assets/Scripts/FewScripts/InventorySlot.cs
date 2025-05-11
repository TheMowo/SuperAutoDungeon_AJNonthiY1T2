using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //This checks if the item slot is empty
        if (transform.childCount == 0)
        {
            // If the item slot is empty, drop the item in the slot

            // Gets the item being dragged
            GameObject droppedItem = eventData.pointerDrag;
            InventoryItem draggableItem = droppedItem.GetComponent<InventoryItem>();

            // Drops the item in the slot
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            // If the item slot is occupied by another item, swap them
            
            // Gets the item being dragged
            GameObject droppedItem = eventData.pointerDrag;
            InventoryItem draggableItem = droppedItem.GetComponent<InventoryItem>();

            // Gets the current item in the slot
            GameObject current = transform.GetChild(0).gameObject;
            InventoryItem currentDraggable = current.GetComponent<InventoryItem>();

            // Swaps the items
            currentDraggable.transform.SetParent(draggableItem.parentAfterDrag);
            draggableItem.parentAfterDrag = transform;
        }
    }

    public ItemSaveData GetDataSave()
    {
        if (transform.childCount != 0)
        {
            return new ItemSaveData
            {
                SlotIndex = this.transform.GetSiblingIndex(),
                image = this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite,
                item = this.transform.GetChild(0).gameObject.GetComponent<InventoryItem>().CurrentItem
            };
        }
        else
        {
            return new ItemSaveData 
            {
                SlotIndex = this.transform.GetSiblingIndex()
            };

        }
    }

    public void LoadFromSaveData(ItemSaveData data)
    {
        this.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = data.image;
        this.transform.GetChild(0).gameObject.GetComponent<InventoryItem>().CurrentItem = data.item;
        this.transform.GetChild(0).gameObject.GetComponent<TooltipTrigger>().currentItem = data.item;
    }
}
