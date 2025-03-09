using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ConsumableItem currentItem; // ScriptableObject item's header and content displayed in the tooltip
    public TooltipSystem tooltipSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipSystem.Show(currentItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSystem.Hide();
    }

    [Obsolete]
    public void GetItemData(ConsumableItem itemData)
    {
        currentItem = itemData;
        tooltipSystem = FindObjectOfType<TooltipSystem>();
    }
}
