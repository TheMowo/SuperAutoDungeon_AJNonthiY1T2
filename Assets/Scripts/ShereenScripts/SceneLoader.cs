using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ItemSaveSystem ISS;
    public PlayerSaveSystem Player;

    public void LoadNextScene()
    {
        ISS = FindFirstObjectByType<ItemSaveSystem>();
        Player = FindFirstObjectByType<PlayerSaveSystem>();
        
        ISS.ItemSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        Player.PlayerSaveData();
        SceneManager.LoadScene("Stage2");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
