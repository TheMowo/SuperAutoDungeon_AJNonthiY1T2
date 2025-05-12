using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;
using System.Linq;
using UnityEditor.Overlays;
using Unity.VisualScripting;

public class PlayerSaveSystem : MonoBehaviour
{
    public List<PlayerUnit> allPlayers;
    private string PlayerSavePath => Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        PlayerLoad();
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
        Debug.Log("Player Saved!");
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
        Debug.Log("Player Loaded!");
    }
}
