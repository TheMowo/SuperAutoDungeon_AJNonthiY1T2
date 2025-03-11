using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public List<PlayerUnit> playerUnits;
    public List<EnemiesUnit> enemyUnits;
    public Transform battlePoint;
    public int maxTurns = 5;

    private int currentTurn = 0;

    public void StartBattle()
    {
        if (playerUnits.Count > 0 && enemyUnits.Count > 0)
        {
            StartCoroutine(BattleRoutine());
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

            yield return StartCoroutine(PlayerAttackPhase());

            if (enemyUnits.Count == 0)
            {
                Debug.Log("Player Wins");
                ResetPlayerHP();
                yield break;
            }

            yield return StartCoroutine(EnemyAttackPhase());

            if (playerUnits.Count == 0)
            {
                Debug.Log("Enemy Wins");
                yield break;
            }

            currentTurn++;
            yield return new WaitForSeconds(1.0f);
        }

        if (enemyUnits.Count == 0)
        {
            Debug.Log("Player Wins");
            ResetPlayerHP();
        }
        else if (playerUnits.Count == 0)
        {
            Debug.Log("Enemy Wins");
        }
        else
        {
            Debug.Log("No one died, Player Wins");
        }
    }

    private IEnumerator PlayerAttackPhase()
    {
        if (enemyUnits.Count == 0) yield break;

        int totalPlayerDamage = 0;
        foreach (var player in playerUnits)
        {
            if (player != null)
            {
                totalPlayerDamage += player.ATK;
            }
        }

        if (enemyUnits.Count > 0 && enemyUnits[0] != null)
        {
            EnemiesUnit enemy = enemyUnits[0];
            enemy.HP -= totalPlayerDamage;
            Debug.Log("Player attacks for " + totalPlayerDamage + " damage");

            if (enemy.HP <= 0)
            {
                Debug.Log(enemy.name + " has died");
                Destroy(enemy.gameObject);
                enemyUnits.RemoveAt(0);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator EnemyAttackPhase()
    {
        if (playerUnits.Count == 0)
        {
            yield break;
        }

        int totalEnemyDamage = 0;
        foreach (var enemy in enemyUnits)
        {
            if (enemy != null)
            {
                totalEnemyDamage += enemy.ATK;
            }
        }

        if (playerUnits.Count > 0 && playerUnits[0] != null)
        {
            PlayerUnit player = playerUnits[0];
            player.HP -= totalEnemyDamage;
            Debug.Log("Enemy attacks for " + totalEnemyDamage + " damage");

            if (player.HP <= 0)
            {
                Debug.Log(player.name + " has died");
                Destroy(player.gameObject);
                playerUnits.RemoveAt(0);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }


    private void ResetPlayerHP()
    {
        foreach (var player in playerUnits)
        {
            if (player != null)
            {
                player.HP = player.GetComponent<PlayerUnitType>().HP;
                Debug.Log(player.name + " HP Reset to " + player.HP);
            }
        }
    }
}
