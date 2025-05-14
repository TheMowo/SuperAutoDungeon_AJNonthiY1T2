using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton_Continue : MonoBehaviour
{
    // Check if a save file exists
    // Enable
    // When enabled continues game based on last saved data
    private bool hasSave = false;

    GameSettingSaveSystem gameSettingSaveSystem;

    private void CheckSave()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        if (File.Exists(savePath))
        {
            hasSave = true;
        }
    } //checks if there is an existing save file

    private void Start()// For enabling continue button
    {
        gameSettingSaveSystem = FindFirstObjectByType<GameSettingSaveSystem>();
        CheckSave();
        if (hasSave)
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    private void LoadLastPlayedScene()
    {
        SceneManager.LoadScene(gameSettingSaveSystem.SceneIndex);
    }
}
