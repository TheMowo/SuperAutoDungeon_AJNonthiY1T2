using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public List<PlayerUnit> allPlayers;
    public List<InventorySlot> Inventoryslot;
    private string PlayerSavePath => Path.Combine(Application.persistentDataPath, "game_save.json");
    private string ItemSavePath => Path.Combine(Application.persistentDataPath, "game_save.json");
    public string fileName;
    private FileDataHandler dataHandler;
    public GameObject ItemPrefab;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        PlayerLoad();
        ItemLoad();
    }

    //Player SaveLoad Parts
    public void PlayerSaveData()
    {
        //Do somthing
    }

    public void PlayerLoad()
    {
        //Do somthing
    }


    //Item SaveLoad Parts
    public void ItemSaveData()
    {
        //Do somthing
    }

    public void ItemLoad()
    {
        //Do somthing
    }

    private void ClearAllInventorySlots()
    {
        //Do somthing
    }

    private void ClearSlotChildren(GameObject slot)
    {
        //Do somthing
    }
}
