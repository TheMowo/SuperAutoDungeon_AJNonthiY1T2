using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

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
    public void Show(ConsumableItem currentItem)
    {
        current.tooltip.SetText(currentItem);
        //current.tooltip.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
    }
    public void Hide()
    {
        //current.tooltip.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }
}
