using UnityEngine;

public class CheckPlayerLose : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private GameObject PlayerLosePrefab;

    public bool isOver = false;
    void Update()
    {
        if(combatSystem.playerUnits[1].isDead && isOver == false)
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
