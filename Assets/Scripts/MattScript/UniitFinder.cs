using UnityEngine;

public class UniitFinder : MonoBehaviour
{
    public EnemySaveSystem enemySaveSystem;
    public PlayerSaveSystem playerSaveSystem;
    public ItemSaveSystem itemSaveSystem;
    public CurrencySaveSystem currencySaveSystem;
    public GameSettingSaveSystem gameSettingSaveSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSaveSystem = FindFirstObjectByType<ItemSaveSystem>();
        playerSaveSystem = FindFirstObjectByType<PlayerSaveSystem>();
        enemySaveSystem = FindFirstObjectByType<EnemySaveSystem>();
        currencySaveSystem = FindFirstObjectByType<CurrencySaveSystem>();
        gameSettingSaveSystem = FindFirstObjectByType<GameSettingSaveSystem>();
        if (enemySaveSystem != null)
        {
            gameSettingSaveSystem.combatSystem = FindFirstObjectByType<CombatSystem>();
            itemSaveSystem.GetAllInventorySlotList();
            enemySaveSystem.GetAllPlayerUnitList();
            playerSaveSystem.GetAllPlayerUnitList();
            gameSettingSaveSystem.GetSaveData();
            playerSaveSystem.PlayerLoad();
            itemSaveSystem.ItemLoad();
            currencySaveSystem.CurrencyLoad();
        }
    }
}
