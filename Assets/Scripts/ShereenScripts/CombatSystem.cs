﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private TurnCountUI turnCountUI;
    [SerializeField] private SpawnAnimatedUI spawnAnimatedUI;
    public TMP_Text atkDisplayText;
    public InventoryManager inventory;
    public List<PlayerUnit> playerUnits;
    public List<EnemiesUnit> enemyUnits;

    public List<Transform> playerPositions;
    public List<Transform> enemyPositions;
    public Transform battlePoint;
    public enemySpawner enemySpawner;
    public ShopManager shopManager;
    public bool isWin;

    public PlayerSaveSystem PSS;
    public ItemSaveSystem TSS;
    public ShopSaveSystem SSS;
    public CurrencySaveSystem CSS;

    public float attackSpeed = 3.0f;

    [SerializeField] private int maxTurns = 5;
    public int currentTurn = 1;

    [SerializeField] AudioSource swordmanAttackAudio_;

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
                PSS.PlayerSaveData();
                TSS.ItemSaveData();
                CSS.CurrencySaveData();
                
                Debug.Log("Player Wins");
                ResetPlayerHP();
                RepositionUnits();
                isWin = true;

                shopManager.OpenShopOnWin();
                SSS.GetAllShopSlotList();
                SSS.ShopoSaveData();

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
            turnCountUI.UpdateTurnsUI();
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
            PSS.PlayerSaveData();
            TSS.ItemSaveData();
            CSS.CurrencySaveData();

            Debug.Log("No one died, Player Wins");
            ResetPlayerHP();
            RepositionUnits();
            isWin = true;

            shopManager.OpenShopOnWin();
            SSS.GetAllShopSlotList();
            SSS.ShopoSaveData();
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
                    spawnAnimatedUI.PlayerAttackAnimationAt(0, 0);
                    //SoundManager.PlaySfxClipWithPitchChange(swordmanAttackAudio_);
            }
            else if (attacker.playerType == PlayerType.Bow)
            {
                int baseTargetIndex = 2 - i; //Next 2 slots

                for (int t = baseTargetIndex; t >= 0; t--)
                {
                    if (t < enemyUnits.Count)
                    {
                        target = enemyUnits[t];
                        spawnAnimatedUI.PlayerAttackAnimationAt(t, 1);
                        //SoundManager.PlaySfxClipWithPitchChange(bowmanAttackAudio_);
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

            spawnAnimatedUI.EnemyAttackAnimationAt(0, 1);
            targetPlayer.UpdateUI();

            Debug.Log("Total Enemy ATK = " + totalATK);

            if (targetPlayer.BasedHP + targetPlayer.CurrentHP <= 0)
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

    void Update()
    {
        PlayerUnit targetPlayer = playerUnits[0];
        if (targetPlayer.BasedHP + targetPlayer.CurrentHP <= 0)
        {
            Debug.Log($"{targetPlayer.name} has died");
            playerUnits.RemoveAt(0);
            Destroy(targetPlayer.gameObject);
            RepositionUnits();
        }
        
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            EnemiesUnit attacker = enemyUnits[i];
            if (attacker.HP <= 0)
            {
                Debug.Log($"{attacker.name} has died");
                enemyUnits.Remove(attacker);
                inventory.playerCurrency += attacker.enemiesUnitType.DropGold;
                inventory.UpdateCurrencyUI();
                Destroy(attacker.gameObject);
                RepositionUnits();
            }
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
                //player.BasedHP = player.playerUnitType.HP;
                //player.BasedATK = player.playerUnitType.ATK;
                player.CurrentATK = 0;
                player.CurrentHP = 0;
                Debug.Log(player.name + " HP Reset to " + player.BasedHP);
            }
        }
    }
    public void SpawnEnemies()
    {
        enemySpawner.SpawnEnemies(enemyUnits);
    }
}
