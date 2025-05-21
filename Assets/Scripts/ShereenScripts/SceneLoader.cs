using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ItemSaveSystem ISS;
    public CurrencySaveSystem CSS;
    public PlayerSaveSystem Player;
    public GameSettingSaveSystem GSS;

    public void LoadNextScene()
    {
        ISS = FindFirstObjectByType<ItemSaveSystem>();
        Player = FindFirstObjectByType<PlayerSaveSystem>();
        CSS = FindFirstObjectByType<CurrencySaveSystem>();
        GSS.combatSystem = FindFirstObjectByType<CombatSystem>();

        if (ISS != null)
        {
            ISS.ItemSaveData();
            CSS.CurrencySaveData();
            Player.PlayerSaveData();
            GSS.GetSaveData();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
