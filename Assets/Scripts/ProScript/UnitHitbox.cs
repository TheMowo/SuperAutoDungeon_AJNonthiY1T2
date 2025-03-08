using UnityEngine;
using UnityEngine.EventSystems;

public class UnitHitbox : MonoBehaviour , IDropHandler
{
    [SerializeField] GameObject TargetUnit;
    public PlayerUnit playerUnit;
    public EnemiesUnit enemiesUnit;
    public ConsumableItem SelectedItem;
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

        if (isPlayerUnit)
        {
            playerUnit.UseConsumable(draggableItem.CurrentItem);
            Destroy(droppedItem);
        }
        else
        {
            enemiesUnit.UseConsumable(draggableItem.CurrentItem);
            Destroy(droppedItem);
        }
    }
}
