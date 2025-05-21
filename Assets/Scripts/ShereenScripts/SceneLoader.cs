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
        GSS = FindFirstObjectByType<GameSettingSaveSystem>();
        GSS.gameSettingSaveData = FindFirstObjectByType<GameSettingSaveSystem>();

        if (ISS != null)
        {
            GSS.combatSystem = FindFirstObjectByType<CombatSystem>();
            ISS.ItemSaveData();
            CSS.CurrencySaveData();
            Player.PlayerSaveData();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
