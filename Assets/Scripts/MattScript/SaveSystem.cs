using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;
using System.Linq;
using UnityEditor.Overlays;
using Unity.VisualScripting;

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

    public void PlayerSaveDataWhen()
    {
        PlayerSaveData();
    }

    public void ItemSaveDataWhen()
    {
        ItemSaveData();
    }

    public void PlayerSaveData()
    {
        var saveData = new GameSaveData();
        foreach (var player in allPlayers)
        {
            saveData.players.Add(player.GetSaveData());
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(PlayerSavePath, json);
        Debug.Log("Game Saved!");
    }

    public void ItemSaveData()
    {
        var saveData = new GameSaveData();
        foreach (var inventory in Inventoryslot)
        {
            saveData.items.Add(inventory.GetDataSave());
        }

        string json2 = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(ItemSavePath, json2);
        Debug.Log("Game Saved!");
    }

    public void PlayerLoad()
    {
        if (!File.Exists(PlayerSavePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(PlayerSavePath);
        Debug.Log("Loaded JSON:\n" + json);

        var saveData = JsonUtility.FromJson<GameSaveData>(json);
        foreach (var data in saveData.players)
        {
            var matchingPlayer = allPlayers.Find(p => p.uniqueID == data.uniqueID);
            matchingPlayer.LoadFromSaveData(data);
            Debug.Log(matchingPlayer.uniqueID);
        }
        Debug.Log("Game Loaded!");
    }

    public void ItemLoad()
    {
        if (!File.Exists(ItemSavePath))
        {
            Debug.LogWarning("No save file found. Clearing all inventory slots.");
            ClearAllInventorySlots();
            return;
        }

        string json = File.ReadAllText(ItemSavePath);
        Debug.Log("Loaded JSON:\n" + json);

        var saveData = JsonUtility.FromJson<GameSaveData>(json);

        // Create dictionary for faster slot lookup
        Dictionary<int, ItemSaveData> slotDataDict = saveData.items.ToDictionary(item => item.SlotIndex, item => item);

        foreach (var slot in Inventoryslot)
        {
            int slotIndex = slot.transform.GetSiblingIndex();

            if (slotDataDict.TryGetValue(slotIndex, out var slotData) && slotData.item != null)
            {
                // Slot has saved data
                bool hasChild = slot.transform.childCount > 0;

                if (hasChild)
                {
                    Debug.Log($"Loading data into existing item in slot {slotIndex}");
                    slot.LoadFromSaveData(slotData);
                }
                else
                {
                    // Create new child and load data
                    Debug.Log($"Creating new item in slot {slotIndex}");
                    GameObject newItem = Instantiate(ItemPrefab, slot.transform);
                    newItem.transform.localPosition = Vector3.zero;
                    slot.LoadFromSaveData(slotData);
                }
            }
            else
            {
                // Slot has no saved data
                bool hasChild = slot.transform.childCount > 0;

                if (hasChild)
                {
                    // Remove existing child
                    Debug.Log($"Clearing slot {slotIndex} with no saved data");
                    ClearSlotChildren(slot.gameObject);
                }
                // Else: Do nothing (no saved data and no child)
            }
        }
        Debug.Log("Inventory Loaded!");
    }

    private void ClearAllInventorySlots()
    {
        foreach (var slot in Inventoryslot)
        {
            ClearSlotChildren(slot.gameObject);
        }
    }

    private void ClearSlotChildren(GameObject slot)
    {
        // Destroy all children of the slot
        foreach (Transform child in slot.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
