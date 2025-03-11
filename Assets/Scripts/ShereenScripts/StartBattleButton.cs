using UnityEngine;
using UnityEngine.UI;

public class StartBattleButton : MonoBehaviour
{
    public CombatSystem combatSystem; // อ้างอิงถึง CombatSystem
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
        // ถ้าฝั่ง Player หรือ Enemy ไม่มีตัวเลย → ปุ่มจะกดไม่ได้
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
        button.interactable = false; // ปิดปุ่มหลังจากกดเริ่มสู้
    }
}
