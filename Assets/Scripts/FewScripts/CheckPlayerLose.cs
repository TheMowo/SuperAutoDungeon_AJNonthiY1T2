using UnityEngine;

public class CheckPlayerLose : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private GameObject PlayerLosePrefab;

    bool isOver = false;
    void Update()
    {
        if(combatSystem.playerUnits.Count == 0 && isOver == false)
        {
            TriggerLose();
        }
    }

    void TriggerLose()
    {
        Delay.Run(2, () => Instantiate(PlayerLosePrefab));
        isOver =true;
    }


}
