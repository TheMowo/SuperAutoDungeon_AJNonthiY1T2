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

            foreach (var unit in playerUnits)
            {
                if (unit.CurrentEffects.Contains(DebuffEffectType.Poison))
                {
                    unit.CurrentHP -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.PoisonII))
                {
                    unit.CurrentHP /= 2;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Weakness))
                {
                    unit.CurrentATK -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.WeaknessII))
                {
                    unit.CurrentATK /= 2;
                }
            }

            foreach (var unit in enemyUnits)
            {
                if (unit.CurrentEffects.Contains(DebuffEffectType.Poison))
                {
                    unit.HP -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.PoisonII))
                {
                    unit.HP /= 2;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Weakness))
                {
                    unit.ATK -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.WeaknessII))
                {
                    unit.ATK /= 2;
                }
            }

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
            if (attacker.CurrentEffects.Contains(DebuffEffectType.Frozen))
            {
                continue;
            }
            else if (attacker.CurrentEffects.Contains(DebuffEffectType.Slowness))
            {
                if (attacker.TurnSkipSlow == 0)
                {
                    attacker.TurnSkipSlow += 1;
                    continue;
                }
                else attacker.TurnSkipSlow = 0;
            }

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
                int baseTargetIndex = 2 - i; //Next 2 slots

                for (int t = baseTargetIndex; t >= 0; t--)
                {
                    if (t < enemyUnits.Count)
                    {
                        target = enemyUnits[t];
                        break;
                    }
                }
            }


            if (target != null)
            {
                if (target.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
                {
                    target.HP -= (attacker.BasedATK + attacker.CurrentATK) * 2;
                }
                else target.HP -= attacker.BasedATK + attacker.CurrentATK;

                if (target.CurrentEffects.Contains(DebuffEffectType.Lethal) && attacker.BasedATK > 0)
                {
                    target.HP = 0;
                }

                target.UpdateUI();

                if (atkDisplayText != null)
                    atkDisplayText.text = $"{attacker.BasedATK + attacker.CurrentATK}";

                Debug.Log($"{attacker.name} attacked {target.name} for {attacker.BasedATK + attacker.CurrentATK} damage");

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
            if (attacker.CurrentEffects.Contains(DebuffEffectType.Frozen))
            {
                continue;
            }
            else if (attacker.CurrentEffects.Contains(DebuffEffectType.Slowness))
            {
                if (attacker.TurnSkipSlow == 0)
                {
                    attacker.TurnSkipSlow += 1;
                    continue;
                }
                else attacker.TurnSkipSlow = 0;
            }

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
            if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
            {
                targetPlayer.CurrentHP -= totalATK * 2;
            }
            else targetPlayer.CurrentHP -= totalATK;

            if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Lethal) && totalATK > 0)
            {
                targetPlayer.CurrentHP = 0;
            }

            targetPlayer.UpdateUI();

            Debug.Log("Total Enemy ATK = " + totalATK);

            if (targetPlayer.BasedHP <= 0)
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
                player.BasedHP = player.playerUnitType.HP;
                Debug.Log(player.name + " HP Reset to " + player.BasedHP);
            }
        }
    }
    public void SpawnEnemies()
    {
        enemySpawner.SpawnEnemies(enemyUnits);
    }
}
