using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetText(ConsumableItem currentItem)
    {
        if(string.IsNullOrEmpty(currentItem.Name)) //If the header is empty, hide header
        {
            headerField.gameObject.SetActive(false);
        } 
        else // and else is you know what, vice versa.
        {
            headerField.gameObject.SetActive(true);
            headerField.text = currentItem.Name;
        }

        contentField.text = currentItem.Description; //Set the content of the tooltip

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false; 
        // ^ ^ ^ If the text is too long, wrap it to a fixed size ^ ^ ^
    }

    private void Update()
    {
        Vector2 itemPosition = Input.mousePosition;

        float pivotX = itemPosition.x / Screen.width;
        float pivotY = itemPosition.y / Screen.height;  
        
        float finalPivotX = 0f;
        float finalPivotY = 0f;

        if (pivotX < 0.5) //If mouse on left of screen move tooltip to right of cursor and vice vera
        {
            finalPivotX = -0.1f;
        }
        else
        {
            finalPivotX = 1.01f;
        }

        if (pivotY < 0.5) //If mouse on lower half of screen move tooltip above cursor and vice versa
        {
            finalPivotY = 0;
        }
        else
        {
            finalPivotY = 1;
        }

        rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);
        transform.position = itemPosition;
    }
}
