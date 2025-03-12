using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public InventoryManager inventory;
    public List<PlayerUnit> playerUnits;
    public List<EnemiesUnit> enemyUnits;

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;
    public Transform battlePoint;
    public enemySpawner enemySpawner;

    public GameObject[] shopOpenState;
    public GameObject[] shopCloseState;
    public GameObject nextStageButton;
    public ShopManager shopManager;

    public float attackSpeed = 3.0f;

    [SerializeField] private int maxTurns = 5;
    private int currentTurn = 0;

    public void Awake()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
    }
    public void StartBattle()
    {
        if (playerUnits.Count > 0 && enemyUnits.Count > 0)
        {
            SetStartPositions();
            StartCoroutine(BattleRoutine());
        }
    }

    private void SetStartPositions()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (i < playerPositions.Count)
                playerUnits[i].transform.position = playerPositions[i].position;
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (i < enemyPositions.Count)
                enemyUnits[i].transform.position = enemyPositions[i].position;
        }
    }

    private IEnumerator BattleRoutine()
    {
        currentTurn = 0;
        while (currentTurn < maxTurns)
        {
            if (playerUnits.Count == 0 || enemyUnits.Count == 0)
                break;

            Debug.Log("Turn " + (currentTurn + 1));

            // Players attack first
            yield return StartCoroutine(PlayerAttackPhase());
            // then checks if player killed off all enemies
            if (enemyUnits.Count == 0)
            {
                Debug.Log("Player Wins");
                ResetPlayerHP();

                RepositionUnits();

                shopManager.AddRandomItems(8);
                foreach (var shopO in shopOpenState)
                {
                    shopO.SetActive(true);
                }
                foreach (var shopC in shopCloseState)
                {
                    shopC.SetActive(false);
                }
                nextStageButton.SetActive(true);

                yield break;
            }
            yield return new WaitForSeconds(1.0f);
            // Enemies attack second
            yield return StartCoroutine(EnemyAttackPhase());
            // then checks if enemy killed off all players
            if (playerUnits.Count == 0)
            {
                Debug.Log("Enemy Wins");
                yield break;
            }

            RepositionUnits();
            currentTurn++;
            yield return new WaitForSeconds(1.0f);
        }

        if (enemyUnits.Count == 0)
        {
            Debug.Log("Player Wins");
            ResetPlayerHP(); // Player wins, health back to same value
            
            RepositionUnits();
        }
        else if (playerUnits.Count == 0)
        {
            Debug.Log("Enemy Wins");
            RepositionUnits();
        }
        else
        {
            Debug.Log("No one died, Player Wins");
            ResetPlayerHP();

            RepositionUnits();

            shopManager.AddRandomItems(8);
            foreach (var shopO in shopOpenState)
                {
                    shopO.SetActive(true);
                }
                foreach (var shopC in shopCloseState)
                {
                    shopC.SetActive(false);
                }
            nextStageButton.SetActive(true);
        }
    }

    private IEnumerator PlayerAttackPhase()
    {
        if (playerUnits.Count == 0 || enemyUnits.Count == 0)
            yield break;

        int playerAttackers = Mathf.Min(3, playerUnits.Count); // Up to 3 players attack

        // Move attacking players to battle point one by one to middle
        for (int i = 0; i < playerAttackers; i++)
        {
            yield return StartCoroutine(MoveToPosition(playerUnits[i].gameObject, battlePoint.position));
        }

        yield return new WaitForSeconds(0.5f);

        // Players attack first enemy
        if (enemyUnits.Count > 0)
        {
            EnemiesUnit targetEnemy = enemyUnits[0];

            for (int i = 0; i < playerAttackers; i++)
            {
                PlayerUnit attacker = playerUnits[i];
                targetEnemy.HP -= attacker.ATK;

                Debug.Log($"{attacker.name} attacks {targetEnemy.name} for {attacker.ATK} damage");

                yield return new WaitForSeconds(0.2f);
                targetEnemy.UpdateUI();

                if (targetEnemy.HP <= 0)
                {
                    Debug.Log($"{targetEnemy.name} has died");
                    enemyUnits.RemoveAt(0);
                    inventory.playerCurrency += targetEnemy.enemiesUnitType.DropGold;
                    Debug.Log($"{inventory.playerCurrency} + {targetEnemy.enemiesUnitType.DropGold}");
                    inventory.UpdateCurrencyUI();
                    RepositionUnits();
                    Destroy(targetEnemy.gameObject);
                    yield return new WaitForSeconds(0.5f);
                    yield break;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Move attacking players back to their original positions
        for (int i = 0; i < playerAttackers; i++)
        {
            yield return StartCoroutine(MoveToPosition(playerUnits[i].gameObject, playerPositions[i].position));
        }
        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator EnemyAttackPhase()
    {
        if (enemyUnits.Count == 0 || playerUnits.Count == 0)
        yield break;

        int enemyAttackers = Mathf.Min(3, enemyUnits.Count); // Up to 3 enemies attack

        // Move attacking enemies to battle point
        for (int i = 0; i < enemyAttackers; i++)
        {
            yield return StartCoroutine(MoveToPosition(enemyUnits[i].gameObject, battlePoint.position));
        }
        yield return new WaitForSeconds(0.5f);

        // Enemies attack first player
        if (playerUnits.Count > 0)
        {
            PlayerUnit targetPlayer = playerUnits[0];

            for (int i = 0; i < enemyAttackers; i++)
            {
                EnemiesUnit attacker = enemyUnits[i];
                targetPlayer.HP -= attacker.ATK;
                targetPlayer.UpdateUI();

                Debug.Log($"{attacker.name} attacks {targetPlayer.name} for {attacker.ATK} damage");

                if (targetPlayer.HP <= 0)
                {
                    Debug.Log($"{targetPlayer.name} has died");
                    playerUnits.RemoveAt(0);
                    Destroy(targetPlayer.gameObject);
                    yield break;
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Move attacking enemies back to their original positions
        for (int i = 0; i < enemyAttackers; i++)
        {
            yield return StartCoroutine(MoveToPosition(enemyUnits[i].gameObject, enemyPositions[i].position));
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveToPosition(GameObject unit, Vector3 target)
    {
        while (unit != null && Vector2.Distance(unit.transform.position, target) > 0.01f)
        {
            unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, attackSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void RepositionUnits() 
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (i < playerPositions.Count)
                playerUnits[i].transform.position = playerPositions[i].position;
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (i < enemyPositions.Count)
                enemyUnits[i].transform.position = enemyPositions[i].position;
        }
    }

    private void ResetPlayerHP()
    {
        foreach (var player in playerUnits)
        {
            if (player != null)
            {
                player.HP = player.playerUnitType.HP;
                Debug.Log(player.name + " HP Reset to " + player.HP);
            }
        }
    }
    public void SpawnEnemies()
    {
        enemySpawner.SpawnEnemies(enemyUnits);
    }
}
