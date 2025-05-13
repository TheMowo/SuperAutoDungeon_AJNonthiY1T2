using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton_Continue : MonoBehaviour
{
    // Check if a save file exists
    // Enable
    // When enabled continues game based on last saved data
    private bool hasSave = false;

    private void CheckSave()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        if (File.Exists(savePath))
        {
            hasSave = true;
        }
    } //checks if there is an existing save file

    private void Start()
    {
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

    public void LoadSaveData()
    {

    }
}
