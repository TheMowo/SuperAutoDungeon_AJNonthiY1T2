using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public List<PlayerUnit> allPlayers;
    private string SavePath => Path.Combine(Application.persistentDataPath, "game_save.json");

    private void Awake()
    {
        LoadGame();
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

    public void LoadGame()
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

}
