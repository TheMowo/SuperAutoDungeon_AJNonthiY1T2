using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public UnityEngine.UI.Image image;
    public ConsumableItem CurrentItem;
    public bool isShopItem = false;

    private bool isDragging = false;
    private float floatHeight = 10f; 
    public float followSpeed = 15f;
    public float bobbingSpeed = 2f;
    public float bobbingHeight = 100f;
    public float tiltSpeed = 0.1f;
    public float _tiltAmount = 5f;

    [HideInInspector] public Transform parentAfterDrag;
    
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if(isDragging)
        {
            // I'm commenting on these because I am still trying to learning on this LMAOOO, please don't make fun of me
            
            // This line 26 and 28 makes the item follow the mouse position but with a little delay
            Vector3 mousePos = Input.mousePosition + new Vector3(0, floatHeight, 0);
            Vector3 prevPosition = transform.position;
            transform.position = Vector3.Lerp(transform.position, mousePos, followSpeed * Time.deltaTime);
            
            // This line 31 scale up the item while dragging
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.5f, Time.deltaTime * 10f);

            // This line 34 makes the item bob up and down while dragging
            float bobbing = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight; // speed and height
            transform.position += new Vector3(0, bobbing, 0) * Time.deltaTime;

            Vector3 velocity = (transform.position - prevPosition) / Time.deltaTime;
            // This line 39 makes the item tilt while dragging
            float tiltAmount = Mathf.Clamp(velocity.x * tiltSpeed, -_tiltAmount, _tiltAmount); // Adjust 0.1f, -15/15 for feel
            transform.rotation = Quaternion.Euler(0, 0, -tiltAmount);
        }
        else
        {
            // Reset rotation when not dragging
            transform.rotation = Quaternion.identity;
        }
    }
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
        //transform.position = Input.mousePosition;
        isDragging = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(isShopItem) return;
        
        //Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); 
        image.raycastTarget = true;

        isDragging = false;
        transform.localScale = Vector3.one; // this line scale down the item when you stop dragging
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
