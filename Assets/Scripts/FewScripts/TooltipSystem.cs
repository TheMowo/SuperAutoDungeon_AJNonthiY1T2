using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    private GameObject hoveredObject;

    public Tooltip tooltip;
    public CanvasGroup canvasGroup;

    public void Awake()
    {
        current = this;
        canvasGroup.alpha = 0f;
    }
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Update()
    {
        if (hoveredObject == null)
        {
            Hide();
        }
    }
    public void Show(ConsumableItem currentItem, GameObject sourceObject)
    {
        hoveredObject = sourceObject; // This is the object that the tooltip is hovering over

        current.tooltip.SetText(currentItem);
        canvasGroup.alpha = 1f;
    }
    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
