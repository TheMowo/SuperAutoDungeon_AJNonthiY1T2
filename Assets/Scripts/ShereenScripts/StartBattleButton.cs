using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartBattleButton : MonoBehaviour
{
    public CombatSystem combatSystem;
    public PlayerSaveSystem PSS;
    public EnemySaveSystem ESS;
    public ItemSaveSystem TSS;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartBattle);
        UpdateButtonState();
    }

    void Update()
    {
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        if (combatSystem.playerUnits.Count == 0 || combatSystem.enemyUnits.Count == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    void StartBattle()
    {
        ESS.EnemySaveData();
        PSS.PlayerSaveData();
        TSS.ItemSaveData();
        combatSystem.StartBattle();
        button.interactable = false;
    }
}
