using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ItemSaveSystem ISS;
    public CurrencySaveSystem CSS;
    public PlayerSaveSystem Player;

    public void LoadNextScene()
    {
        ISS = FindFirstObjectByType<ItemSaveSystem>();
        Player = FindFirstObjectByType<PlayerSaveSystem>();
        CSS = FindFirstObjectByType<CurrencySaveSystem>();
        
        ISS.ItemSaveData();
        CSS.CurrencySaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        Player.PlayerSaveData();
    }
}
