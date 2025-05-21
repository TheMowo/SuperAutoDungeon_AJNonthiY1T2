using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingSaveSystem : MonoBehaviour
{
    public GameSettingSaveSystem gameSettingSaveData;
    private string GameSettingSavePath => Path.Combine(Application.persistentDataPath, "GameSettingSaveData.json");
    public string fileName;
    private FileDataHandler dataHandler;
    public CombatSystem combatSystem;
    public int SceneIndex;
    public static GameSettingSaveSystem Instance; private void Awake() { if (Instance != null) { Debug.Log("GameSettingSaveSystem Instance Check !Null"); Destroy(this.gameObject); } else DontDestroyOnLoad(this.gameObject); Instance = this; }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName); //Application.persistentDataPath <== change this to save where ever you want
        Debug.Log(this.dataHandler);
        if (GameObject.Find("Player Unit 1") != null)
        {
            gameSettingSaveData = FindFirstObjectByType<GameSettingSaveSystem>();
            combatSystem = FindFirstObjectByType<CombatSystem>();
            GameSettingLoad();
            Debug.Log("HELP MEMEMEMEMEMEMEMEMEME");
        }
    }

    public void GameSettingSaveData()
    {
        SettingSaveData saveData = gameSettingSaveData.GetSaveData(); // Get currency data
        string json = JsonUtility.ToJson(saveData, true); // Serialize data
        File.WriteAllText(GameSettingSavePath, json); // Save to file
        Debug.Log("GameSetting saved to: " + GameSettingSavePath);
    }

    public void GameSettingLoad()
    {
        if (!File.Exists(GameSettingSavePath))
        {
            Debug.LogWarning("No currency save file found at: " + GameSettingSavePath);
            return;
        }

        string json = File.ReadAllText(GameSettingSavePath);
        Debug.Log("Loaded JSON:\n" + json);

        SettingSaveData loadedData = JsonUtility.FromJson<SettingSaveData>(json);
        gameSettingSaveData.LoadFromSaveData(loadedData); // Apply loaded data
        Debug.Log("GameSetting loaded!");
    }

    public void DeleteSaveFile()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "GameSettingSaveData.json");
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

    public SettingSaveData GetSaveData()
    {
        Debug.Log("Fucking stupit");
        return new SettingSaveData
        {
            PlayerWin = combatSystem.isWin,
            SceneName = SceneManager.GetActiveScene().buildIndex
        };
    }

    public void LoadFromSaveData(SettingSaveData Data)
    {
        combatSystem.isWin = Data.PlayerWin;
        SceneIndex = Data.SceneName;
    }
}
