using System.IO;
using UnityEngine;

public class SettingSaveSystem : MonoBehaviour
{
    public MainMenuController Setting;
    private string SettingSavePath => Path.Combine(Application.persistentDataPath, "SettingSaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;

    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
    }

    private void Awake()
    {
        LoadSettings();
    }

    public void CurrencySaveData()
    {
        SettingSaveData saveData = new SettingSaveData();
        string json = JsonUtility.ToJson(saveData, true); // Serialize data
        File.WriteAllText(SettingSavePath, json); // Save to file
        Debug.Log("Currency saved to: " + SettingSavePath);
    }

    public void SaveSettings()
    {
        SettingSaveData saveData = new SettingSaveData();
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SettingSavePath, json);
        Debug.Log("Settings saved to: " + SettingSavePath);
    }

    public void LoadSettings()
    {
        if (!File.Exists(SettingSavePath))
        {
            Debug.LogWarning("No settings save file found at: " + SettingSavePath);
            return;
        }

        string json = File.ReadAllText(SettingSavePath);
        Debug.Log("Loaded JSON:\n" + json);

        SettingSaveData loadedData = JsonUtility.FromJson<SettingSaveData>(json);
        //Setting.LoadSaveData(loadedData);
        Debug.Log("Settings loaded!");
    }

}
