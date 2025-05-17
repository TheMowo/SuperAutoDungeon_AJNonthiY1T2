using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CurrencySaveSystem : MonoBehaviour
{
    public InventoryManager Currency;
    private string CurrencySavePath => Path.Combine(Application.persistentDataPath, "CurrencySaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keeps this GameObject across scenes
    }

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        if(GameObject.Find("Player Unit 1") == true)
        {
            Currency = FindFirstObjectByType<InventoryManager>();
            CurrencyLoad();
        }
    }

    public void CurrencySaveData()
    {
        CurrencySaveData saveData = Currency.GetDataSave(); // Get currency data
        string json = JsonUtility.ToJson(saveData, true); // Serialize data
        File.WriteAllText(CurrencySavePath, json); // Save to file
        Debug.Log("Currency saved to: " + CurrencySavePath);
    }

    public void CurrencyLoad()
    {
        if (!File.Exists(CurrencySavePath))
        {
            Debug.LogWarning("No currency save file found at: " + CurrencySavePath);
            return;
        }

        string json = File.ReadAllText(CurrencySavePath);
        Debug.Log("Loaded JSON:\n" + json);

        CurrencySaveData loadedData = JsonUtility.FromJson<CurrencySaveData>(json);
        Currency.LoadFromSaveData(loadedData); // Apply loaded data
        //UpdateCurrencyUI();
        Debug.Log("Currency loaded!");
    }

    public void DeleteSaveFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "CurrencySaveData.json");
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("Save file deleted: " + fullPath);
        }
        else
        {
            Debug.LogWarning("No save file found to delete at: " + fullPath);
        }
    }

    public void FindCurrency(InventoryManager inventoryManager)
    {
        Currency = inventoryManager;
    }
}
