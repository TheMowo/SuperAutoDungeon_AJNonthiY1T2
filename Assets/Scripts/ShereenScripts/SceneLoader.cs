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
        Player.PlayerSaveData();
        ISS.ItemSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
