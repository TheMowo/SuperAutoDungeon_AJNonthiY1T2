using UnityEngine;
using UnityEngine.UI;

public class StartBattleButton : MonoBehaviour
{
    public CombatSystem combatSystem;
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
        combatSystem.StartBattle();
        button.interactable = false;
    }
}
