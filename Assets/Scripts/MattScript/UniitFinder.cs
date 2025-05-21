using UnityEngine;

public class UniitFinder : MonoBehaviour
{
    public EnemySaveSystem enemySaveSystem;
    public PlayerSaveSystem playerSaveSystem;
    public ItemSaveSystem itemSaveSystem;
    public CurrencySaveSystem currencySaveSystem;
    public GameSettingSaveSystem GSS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSaveSystem = FindFirstObjectByType<ItemSaveSystem>();
        playerSaveSystem = FindFirstObjectByType<PlayerSaveSystem>();
        enemySaveSystem = FindFirstObjectByType<EnemySaveSystem>();
        currencySaveSystem = FindFirstObjectByType<CurrencySaveSystem>();
        GSS = FindFirstObjectByType<GameSettingSaveSystem>();
        if (enemySaveSystem != null)
        {
            GSS.gameSettingSaveData = FindFirstObjectByType<GameSettingSaveSystem>();
            GSS.combatSystem = FindFirstObjectByType<CombatSystem>();
            itemSaveSystem.GetAllInventorySlotList();
            enemySaveSystem.GetAllPlayerUnitList();
            playerSaveSystem.GetAllPlayerUnitList();
            playerSaveSystem.PlayerLoad();
            itemSaveSystem.ItemLoad();
            currencySaveSystem.CurrencyLoad();
        }
    }
}
