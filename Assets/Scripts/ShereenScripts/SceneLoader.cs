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
        
        if(ISS != null)
        {
            ISS.ItemSaveData();
            CSS.CurrencySaveData();
            Player.PlayerSaveData();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
