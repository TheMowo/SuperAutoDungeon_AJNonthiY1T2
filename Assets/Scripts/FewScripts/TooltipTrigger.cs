using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;

    [Multiline()]
    public string content;
    public TooltipSystem tooltipSystem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipSystem.Show(content,header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSystem.Hide();
    }
}
