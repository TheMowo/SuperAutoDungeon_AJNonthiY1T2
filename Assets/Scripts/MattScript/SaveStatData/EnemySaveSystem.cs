using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;
using System.Linq;
using UnityEditor.Overlays;
using Unity.VisualScripting;

public class EnemySaveSystem : MonoBehaviour
{
    public List<EnemiesUnit> allEnemys;
    private string EnemySavePath => Path.Combine(Application.persistentDataPath, "EnemySaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        EnemyLoad();
    }

    public void EnemySaveData()
    {
        var saveData = new GameSaveData();
        foreach (var enemy in allEnemys)
        {
            saveData.Enemys.Add(enemy.GetSaveData());
        }

        string json3 = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(EnemySavePath, json3);
        Debug.Log("Enemy Saved!");
    }

    public void EnemyLoad()
    {
        if (!File.Exists(EnemySavePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(EnemySavePath);
        Debug.Log("Loaded JSON:\n" + json);

        var saveData = JsonUtility.FromJson<GameSaveData>(json);
        foreach (var data in saveData.Enemys)
        {
            var matchingEnemy = allEnemys.Find(e => e.uniqueID == data.uniqueID);
            matchingEnemy.LoadFromSaveData(data);
            Debug.Log(matchingEnemy.uniqueID);
        }
        Debug.Log("Enemy Loaded!");
    }
}
