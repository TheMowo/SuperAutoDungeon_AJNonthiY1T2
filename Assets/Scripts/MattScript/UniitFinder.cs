using UnityEngine;

public class UniitFinder : MonoBehaviour
{
    public EnemySaveSystem enemySaveSystem;
    public PlayerSaveSystem playerSaveSystem;
    public ItemSaveSystem itemSaveSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySaveSystem = FindFirstObjectByType<EnemySaveSystem>();
        enemySaveSystem.GetAllPlayerUnitList();
        playerSaveSystem = FindFirstObjectByType<PlayerSaveSystem>();
        playerSaveSystem.GetAllPlayerUnitList();
        itemSaveSystem = FindFirstObjectByType<ItemSaveSystem>();
        itemSaveSystem.GetAllInventorySlotList();
    }
}
