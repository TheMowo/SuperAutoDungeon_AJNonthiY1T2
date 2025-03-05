using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public List<GameObject> playerUnits;
    public List<GameObject> enemyUnits;
    public Transform battlePoint;

    private int playerIndex = 0;
    private int enemyIndex = 0;

    public void StartBattle()
    {
        if (playerUnits.Count > 0 && enemyUnits.Count > 0)
        {
            StartCoroutine(FightLoop());
        }
    }

    private IEnumerator FightLoop()
    {
        while (playerIndex < playerUnits.Count && enemyIndex < enemyUnits.Count)
        {
            GameObject player = playerUnits[playerIndex];
            GameObject enemy = enemyUnits[enemyIndex];

            if (player == null)
            {
                playerIndex += 1;
                continue;
            }
            if (enemy == null)
            {
                enemyIndex += 1;
                continue;
            }

            //เดินเข้าหากัน
            StartCoroutine(MoveToMiddle(player, battlePoint.position));
            yield return StartCoroutine(MoveToMiddle(enemy, battlePoint.position));

            //รอให้ตลคไปตรงกลาง
            while (player != null && enemy != null && Vector2.Distance(player.transform.position, enemy.transform.position) > 0.5f)
            {
                yield return null;
            }

            if (player != null && enemy != null)
            {
                StartFight(player, enemy);

                while (player != null && enemy != null)
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Battle End");
    }

    private IEnumerator MoveToMiddle(GameObject unit, Vector3 target)
    {
        float speed = 3.0f;
        while (unit != null && Vector2.Distance(unit.transform.position, target) > 0.5f)
        {
            unit.transform.position = Vector2.MoveTowards(unit.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void StartFight(GameObject player, GameObject enemy)
    {
        if (player != null && enemy != null)
        {
            Health_ForTest playerHealth = player.GetComponent<Health_ForTest>();
            Health_ForTest enemyHealth = enemy.GetComponent<Health_ForTest>();

            AutoAttack playerAttack = player.GetComponent<AutoAttack>();
            AutoAttack enemyAttack = enemy.GetComponent<AutoAttack>();

            if (playerHealth != null && enemyHealth != null && playerAttack != null && enemyAttack != null)
            {
                playerAttack.StartCombat(enemyHealth);
                enemyAttack.StartCombat(playerHealth);
            }
        }
    }
}
