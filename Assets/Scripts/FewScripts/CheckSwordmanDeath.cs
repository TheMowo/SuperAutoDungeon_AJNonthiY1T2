using UnityEngine;

public class CheckSwordmanDeath : MonoBehaviour
{
    [SerializeField] private CombatSystem combatSystem;
    [SerializeField] private GameObject swordmanPrefab;
    [SerializeField] private Transform unitParentUI;

    public void RespawnSwordmanIfMissing()
    {
        // Check if Swordman is missing
        bool hasSwordman = combatSystem.playerUnits.Exists(p => p.playerType == PlayerType.Sword);
        bool hasBowman = combatSystem.playerUnits.Exists(p => p.playerType == PlayerType.Bow);

        if (!hasSwordman && hasBowman)
        {
            Debug.Log("Respawning Swordman...");

            // Find the bowman's current position
            PlayerUnit bowman = combatSystem.playerUnits.Find(p => p.playerType == PlayerType.Bow);
            int bowmanIndex = combatSystem.playerUnits.IndexOf(bowman);

            // Instantiate a new Swordman (use your prefab)
            GameObject newSwordmanGO = Instantiate(swordmanPrefab, combatSystem.playerPositions[0].position, Quaternion.identity, unitParentUI);
            PlayerUnit newSwordman = newSwordmanGO.GetComponent<PlayerUnit>();
            
            newSwordman.CurrentHP = 0;
            newSwordman.CurrentATK = 0;

            // Insert Swordman at front (index 0)
            combatSystem.playerUnits.Insert(0, newSwordman);

            // Reposition everything
            combatSystem.RepositionUnits();
        }
    }
}
