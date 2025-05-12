using UnityEngine;
using UnityEngine.EventSystems;

public class UnitHitbox : MonoBehaviour , IDropHandler
{
    [SerializeField] GameObject TargetUnit;
    public PlayerUnit playerUnit;
    public EnemiesUnit enemiesUnit;
    public bool isPlayerUnit;
    public void Awake()
    {
        if (TargetUnit.TryGetComponent<PlayerUnit>(out PlayerUnit Target))
        {
            playerUnit = Target;
            isPlayerUnit = true;
        }
        else if (TargetUnit.TryGetComponent<EnemiesUnit>(out EnemiesUnit Target1))
        {
            enemiesUnit = Target1;
            isPlayerUnit = false;
        }

    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        InventoryItem draggableItem = droppedItem.GetComponent<InventoryItem>();
        draggableItem.parentAfterDrag = transform;

        StatPopUpReceiver receiver = TargetUnit.GetComponent<StatPopUpReceiver>();
        ConsumableItem SOItemStat = draggableItem.CurrentItem;

        if (isPlayerUnit)
        {
            playerUnit.UseConsumable(draggableItem.CurrentItem);
            if (receiver != null)
            {
                receiver.ApplySOStat(SOItemStat);
                //Debug.Log("Receiver Succeed");
                //Debug.Log("TargetUnit: " + TargetUnit.name);
                //Debug.Log("Has receiver: " + (TargetUnit.GetComponent<StatPopUpReceiver>() != null));
            } 
            //else
            //{
                //Debug.Log("Receiver Error");
                //Debug.Log("TargetUnit: " + TargetUnit.name);
                //Debug.Log("Has receiver: " + (TargetUnit.GetComponent<StatPopUpReceiver>() != null));
            //}
            Destroy(droppedItem);
        }
        else
        {
            enemiesUnit.UseConsumable(draggableItem.CurrentItem);

            if (receiver != null)
            {
                receiver.ApplySOStat(SOItemStat);
                Debug.Log("Receiver Succeed");
                Debug.Log("TargetUnit: " + TargetUnit.name);
                Debug.Log("Has receiver: " + (TargetUnit.GetComponent<StatPopUpReceiver>() != null));
            } 
            else
            {
                Debug.Log("Receiver Error");
                Debug.Log("TargetUnit: " + TargetUnit.name);
                Debug.Log("Has receiver: " + (TargetUnit.GetComponent<StatPopUpReceiver>() != null));
            }

            Destroy(droppedItem);
        }
    }
}
