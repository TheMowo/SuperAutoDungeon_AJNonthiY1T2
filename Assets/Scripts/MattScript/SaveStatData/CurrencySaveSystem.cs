using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CurrencySaveSystem : MonoBehaviour
{
    public int Currency;
    private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "CurrencySaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
    }

    public void CurrencySaveData()
    {
        var saveData = new GameSaveData();
        saveData.doubloonsCurrency = Currency;

        string json5 = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(CurrencySavePath, json5);
        Debug.Log("Enemy Saved!");
    }

    public void CurrencyLoad()
    {
        if (!File.Exists(CurrencySavePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json5 = File.ReadAllText(CurrencySavePath);
        Debug.Log("Loaded JSON:\n" + json5);

        var saveData = JsonUtility.FromJson<GameSaveData>(json5);

    }
}
