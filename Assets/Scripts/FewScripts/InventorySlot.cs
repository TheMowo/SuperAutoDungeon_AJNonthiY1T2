using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
       
        return new ItemSaveData
        {
            Order = transform.GetSiblingIndex(),

        };
    }
}
