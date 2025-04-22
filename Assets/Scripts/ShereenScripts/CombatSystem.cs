using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CombatSystem : MonoBehaviour
{
    public TMP_Text atkDisplayText;
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

        for (int i = 0; i < playerUnits.Count; i++)
        {
            PlayerUnit attacker = playerUnits[i];
            if (attacker == null) continue;

            Vector3 originalPos = attacker.transform.position;
            Vector3 popPos = originalPos + Vector3.up * 100f;
            attacker.transform.position = popPos;
            yield return new WaitForSeconds(0.2f);

            EnemiesUnit target = null;

            if (attacker.playerType == PlayerType.Sword)
            {
                if (enemyUnits.Count > 0)
                    target = enemyUnits[0]; //First Enemy
            }
            else if (attacker.playerType == PlayerType.Bow)
            {
                int targetIndex = 2 - i;
                if (targetIndex >= 0 && targetIndex < enemyUnits.Count)
                    target = enemyUnits[targetIndex]; //Next 2 slots
            }

            if (target != null)
            {
                target.HP -= attacker.ATK;
                target.UpdateUI();

                if (atkDisplayText != null)
                    atkDisplayText.text = $"{attacker.ATK}";

                Debug.Log($"{attacker.name} attacked {target.name} for {attacker.ATK} damage");

                if (target.HP <= 0)
                {
                    Debug.Log($"{target.name} has died");
                    enemyUnits.Remove(target);
                    inventory.playerCurrency += target.enemiesUnitType.DropGold;
                    inventory.UpdateCurrencyUI();
                    Destroy(target.gameObject);
                }
            }
            else
            {
                Debug.Log($"{attacker.name} had no valid target");
            }

            yield return new WaitForSeconds(0.3f);
            attacker.transform.position = originalPos;
            yield return new WaitForSeconds(0.2f);
        }

        if (atkDisplayText != null)
            atkDisplayText.text = "";

        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator EnemyAttackPhase()
    {
        if (enemyUnits.Count == 0 || playerUnits.Count == 0)
            yield break;

        int totalATK = 0;

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            EnemiesUnit attacker = enemyUnits[i];
            if (attacker == null) continue;

            Vector3 originalPos = attacker.transform.position;
            Vector3 popPos = originalPos + Vector3.up * 100f;
            attacker.transform.position = popPos;

            yield return new WaitForSeconds(0.3f);

            totalATK += attacker.ATK;

            if (atkDisplayText != null)
            {
                atkDisplayText.text = $"{totalATK}";
            }
            else
            {
                Debug.LogWarning("atkDisplayText is not assigned in the Inspector");
            }
               
            yield return new WaitForSeconds(0.3f);

            attacker.transform.position = originalPos;
            yield return new WaitForSeconds(0.3f);
        }

        if (playerUnits.Count > 0)
        {
            PlayerUnit targetPlayer = playerUnits[0];
            targetPlayer.HP -= totalATK;
            targetPlayer.UpdateUI();

            Debug.Log("Total Enemy ATK = " + totalATK);

            if (targetPlayer.HP <= 0)
            {
                Debug.Log($"{targetPlayer.name} has died");
                playerUnits.RemoveAt(0);
                Destroy(targetPlayer.gameObject);
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (atkDisplayText != null)
        {
            atkDisplayText.text = "";
        }
    }


    //private IEnumerator MoveToPosition(GameObject unit, Vector3 target)
    //{
    //    while (unit != null && Vector2.Distance(unit.transform.position, target) > 0.01f)
    //    {
    //        unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, attackSpeed * Time.deltaTime);
    //        yield return null;
    //    }
    //}

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
