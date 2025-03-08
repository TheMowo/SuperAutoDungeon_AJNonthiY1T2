using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEngine.UI.Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent; 
        transform.SetParent(transform.root); 
        transform.SetAsLastSibling(); 
        image.raycastTarget = false; 
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition; // Move the item with the mouse
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); // Return the item to its original slot
        image.raycastTarget = true; // Make the item interactable again
    }
}
