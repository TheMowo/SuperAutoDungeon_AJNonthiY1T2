using UnityEngine;

public class TurnCountUI : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private TMPro.TextMeshProUGUI turnsUI;

    void Start()
    {
        UpdateTurnsUI();
    }
    public void UpdateTurnsUI()
    {
        Debug.Log("Update Turns UI");
        turnsUI.text = $"TURN: {combatSystem.currentTurn}";
    }
}
