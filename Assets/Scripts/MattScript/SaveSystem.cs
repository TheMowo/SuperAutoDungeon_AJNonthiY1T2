using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;
using System.Linq;
using UnityEditor.Overlays;

public class SaveSystem : MonoBehaviour
{
    public List<PlayerUnit> allPlayers;
    public List<InventorySlot> Inventoryslot;
    private string SavePath => Path.Combine(Application.persistentDataPath, "game_save.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
    }

    private void Awake()
    {
        PlayerLoad();
        ItemLoad();
    }

    private void OnApplicationQuit()
    {
        SaveGamePlayer();
        SaveGameItems();
    }

    public void SaveGamePlayer()
    {
        var saveDataPlayer = new GameSaveData();
        foreach (var player in allPlayers)
        {
            saveDataPlayer.players.Add(player.GetSaveData());
        }

        string json = JsonUtility.ToJson(saveDataPlayer, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Player Saved!");
    }

    public void SaveGameItems()
    {
        var saveDataItem = new GameSaveData();
        foreach (var inventory in Inventoryslot)
        {
            saveDataItem.items.Add(inventory.GetDataSave());
        }

        string json2 = JsonUtility.ToJson(saveDataItem, true);
        File.WriteAllText(SavePath, json2);
        Debug.Log("Items Saved!");
    }

    public void PlayerLoad()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        Debug.Log("Loaded JSON:\n" + json);

        try
        {
            var saveData = JsonUtility.FromJson<GameSaveData>(json);
            foreach (var data in saveData.players)
            {
                var matchingPlayer = allPlayers.Find(p => p.uniqueID == data.uniqueID);
                if (matchingPlayer != null)
                {
                    Debug.Log($"Loading player {data.uniqueID}...");
                    matchingPlayer.LoadFromSaveData(data);
                }
                else
                {
                    Debug.LogWarning($"No matching player for ID {data.uniqueID}");
                }
            }
            Debug.Log("Game Loaded!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Load failed: " + ex.Message);
        }
    }

    public void ItemLoad()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found. Clearing all inventory slots.");
            ClearAllInventorySlots();
            return;
        }

        string json2 = File.ReadAllText(SavePath);
        Debug.Log("Loaded JSON:\n" + json2);

        try
        {
            var saveData = JsonUtility.FromJson<GameSaveData>(json2);

            // First, clear all slots that don't have saved data
            HashSet<int> savedSlotIndices = new HashSet<int>(saveData.items.Select(item => item.SlotIndex));

            foreach (var slot in Inventoryslot)
            {
                int slotIndex = slot.transform.GetSiblingIndex();
                if (!savedSlotIndices.Contains(slotIndex))
                {
                    ClearSlotChildren(slot.gameObject);
                }
            }

            // Then load data for slots that have saved data
            foreach (var slotData in saveData.items)
            {
                var matchingSlot = Inventoryslot.Find(t => t.transform.GetSiblingIndex() == slotData.SlotIndex);
                if (matchingSlot != null)
                {
                    Debug.Log($"Loading itemSlot {slotData.SlotIndex}...");
                    matchingSlot.LoadFromSaveData(slotData);
                }
                else
                {
                    Debug.LogWarning($"No matching itemSlot for Index {slotData.SlotIndex}");
                }
            }
            Debug.Log("Inventory Loaded!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Item load failed: " + ex.Message);
            // If loading fails, clear all slots
            ClearAllInventorySlots();
        }
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
