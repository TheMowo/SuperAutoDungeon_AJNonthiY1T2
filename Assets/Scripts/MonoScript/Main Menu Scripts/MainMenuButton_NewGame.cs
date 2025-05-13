using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton_NewGame : MonoBehaviour
{
    // Check if a save exists
    // If it does, prompt to confirm before proceeding
    // Resets save file
    // Starts new game, transitioning into Scene 1

    [SerializeField] CurrencySaveSystem currencySaveSystem;
    [SerializeField] EnemySaveSystem enemySaveSystem;
    [SerializeField] ItemSaveSystem itemSaveSystem;
    [SerializeField] PlayerSaveSystem playerSaveSystem;
    [SerializeField] ShopSaveSystem shopSaveSystem;

    [SerializeField] GameObject ConfirmationOverlay;
    private bool hasSave;

    public void NewGame_ButtonPressed()
    {
        if (CheckSave())
        {
            PromptConfirmation(true);
        }
        else
        {
            LoadFirstScene();
        }
    } //press new game button
    private bool CheckSave()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "PlayerSaveData.json");
        if (File.Exists(savePath))
        { 
            hasSave = true;
            return true;
        }
        else
        {
            return false;
        }
    } //checks if there is an existing save file

    private void PromptConfirmation(bool enable)
    {
        if(enable)
        {
            ConfirmationOverlay.SetActive(true);
        }
        else
        {
            ConfirmationOverlay.SetActive(false);
        }
    } //self explanatory
    public void ConfirmButton()
    {
        DeleteSaveData();
        PromptConfirmation(false);
        LoadFirstScene();
    } //confirm button
    public void CancelButton()
    {
        PromptConfirmation(false);
    } //cancel button

    private void DeleteSaveData()
    {
        currencySaveSystem.DeleteSaveFile();
        enemySaveSystem.DeleteSaveFile();
        itemSaveSystem.DeleteSaveFile();
        playerSaveSystem.DeleteSaveFile();
        shopSaveSystem.DeleteSaveFile();
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
