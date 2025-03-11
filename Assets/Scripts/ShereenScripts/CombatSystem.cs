using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public List<PlayerUnit> playerUnits;
    public List<EnemiesUnit> enemyUnits;

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;
    public Transform battlePoint;

    [SerializeField] private int maxTurns = 5;

    private int currentTurn = 0;
    private bool isCollided = false;

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

            yield return StartCoroutine(MoveToBattlePoint());

            yield return new WaitUntil(() => isCollided);

            yield return StartCoroutine(AttackPhase());

            if (enemyUnits.Count == 0)
            {
                Debug.Log("Player Wins");
                ResetPlayerHP();
                yield break;
            }

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

    private IEnumerator MoveToBattlePoint()
    {
        isCollided = false;

        if (playerUnits.Count > 0 && enemyUnits.Count > 0)
        {
            PlayerUnit player = playerUnits[0];
            EnemiesUnit enemy = enemyUnits[0];

            StartCoroutine(MoveToPosition(player.gameObject, battlePoint.position));
            yield return StartCoroutine(MoveToPosition(enemy.gameObject, battlePoint.position));
        }
    }

    private IEnumerator MoveToPosition(GameObject unit, Vector3 target)
    {
        float speed = 3.0f;
        while (unit != null && Vector2.Distance(unit.transform.position, target) > 0.5f)
        {
            unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }

        isCollided = true;
    }

    private IEnumerator AttackPhase()
    {
        if (playerUnits.Count == 0 || enemyUnits.Count == 0) yield break;

        if (playerUnits.Count > 0 && enemyUnits.Count > 0)
        {
            PlayerUnit player = playerUnits[0].GetComponent<PlayerUnit>();
            EnemiesUnit enemy = enemyUnits[0].GetComponent<EnemiesUnit>();

            int playerDamage = player.ATK;
            int enemyDamage = enemy.ATK;

            enemy.HP -= playerDamage;
            player.HP -= enemyDamage;

            Debug.Log($"{player.name} attacks {enemy.name} for {playerDamage} damage!");
            Debug.Log($"{enemy.name} attacks {player.name} for {enemyDamage} damage!");

            if (enemy.HP <= 0)
            {
                Debug.Log($"{enemy.name} has died!");
                enemyUnits.RemoveAt(0);
                Destroy(enemy.gameObject);
            }

            if (player.HP <= 0)
            {
                Debug.Log($"{player.name} has died!");
                playerUnits.RemoveAt(0);
                Destroy(player.gameObject);
                RepositionUnits();
            }
        }

        yield return new WaitForSeconds(0.5f);
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
                player.HP = player.GetComponent<PlayerUnitType>().HP;
                Debug.Log(player.name + " HP Reset to " + player.HP);
            }
        }
    }
}
