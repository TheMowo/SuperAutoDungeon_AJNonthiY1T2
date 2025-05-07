using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SaveSystem : MonoBehaviour
{
    public List<PlayerUnit> allPlayers;
    public InventoryManager Inventory;
    private string SavePath => Path.Combine(Application.persistentDataPath, "game_save.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to store in anywhere
        PlayerLoad();
    }

    private void Awake()
    {
        PlayerLoad();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        var saveData = new GameSaveData();
        foreach (var player in allPlayers)
        {
            saveData.players.Add(player.GetSaveData());
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game Saved!");
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

    //public void AddingNewItem()
    //{
    //    var saveData = new GameSaveData();
    //    foreach (var item in Inventory.slots)
    //    {
    //        saveData.items.Add(Inventory.GetSaveData());
    //    }
    //}

    public void ItemLoad()
    {
        
    }
}
