using UnityEngine;

public class UniitFinder : MonoBehaviour
{
    public EnemySaveSystem enemySaveSystem;
    public PlayerSaveSystem playerSaveSystem;
    public ItemSaveSystem itemSaveSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSaveSystem = FindFirstObjectByType<ItemSaveSystem>();
        playerSaveSystem = FindFirstObjectByType<PlayerSaveSystem>();
        enemySaveSystem = FindFirstObjectByType<EnemySaveSystem>();
        itemSaveSystem.GetAllInventorySlotList();
        enemySaveSystem.GetAllPlayerUnitList();
        playerSaveSystem.GetAllPlayerUnitList();
        itemSaveSystem.ItemLoad();
    }
}
