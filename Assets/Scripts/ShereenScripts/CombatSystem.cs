﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;

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

    public float attackSpeed = 3.0f;

    [SerializeField] private int maxTurns = 5;
    public int currentTurn = 1;

    [Header("Audio Files for Sfx")]
    [SerializeField] AudioClip swordmanAttackAudio_;
    [SerializeField] AudioClip bowmanAttackAudio_;

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

            // Players attack first
            yield return StartCoroutine(PlayerAttackPhase());
            // then checks if player killed off all enemies
            if (enemyUnits.Count == 0)
            {
                if(PSS != null)
                {
                    PSS.PlayerSaveData();
                    TSS.ItemSaveData();
                }
                else
                {
                    Debug.LogWarning("PlayerSaveSystem (PSS) is not assigned!");
                }

                Debug.Log("Player Wins");
                ResetPlayerHP();
                RepositionUnits();
                isWin = true;

                shopManager.OpenShopOnWin();
                if (PSS != null)
                {
                    SSS.GetAllShopSlotList();
                    SSS.ShopoSaveData();
                }
                else
                {
                    Debug.LogWarning("PlayerSaveSystem (PSS) is not assigned!");
                }

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

            foreach (var unit in playerUnits)
            {
                if (unit.CurrentEffects.Contains(DebuffEffectType.Poison))
                {
                    unit.CurrentHP -= 1;
                    unit.GreenDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Weakness))
                {
                    if (unit.CurrentATK > 0)
                    {
                        unit.CurrentATK -= 1;
                    }
                    unit.GreyDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Slowness))
                {
                    unit.LightBlueDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
                {
                    unit.GoldDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Shield))
                {
                    unit.ShieldDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.LifeSteal))
                {
                    unit.LifeStealDebuffDuration -= 1;
                }

                unit.CheckDebuff();
            }

            foreach (var unit in enemyUnits)
            {
                if (unit.CurrentEffects.Contains(DebuffEffectType.Poison))
                {
                    unit.HP -= 1;
                    unit.GreenDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Weakness))
                {
                    if (unit.ATK > 0)
                    {
                        unit.ATK -= 1;
                    }
                    unit.GreyDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Slowness))
                {
                    unit.LightBlueDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
                {
                    unit.GoldDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.Shield))
                {
                    unit.ShieldDebuffDuration -= 1;
                }
                if (unit.CurrentEffects.Contains(DebuffEffectType.LifeSteal))
                {
                    unit.LifeStealDebuffDuration -= 1;
                }

                unit.CheckDebuff();
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
            if (PSS != null)
            {
                PSS.PlayerSaveData();
                TSS.ItemSaveData();
            }
            else
            {
                Debug.LogWarning("PlayerSaveSystem (PSS) is not assigned!");
            }

            Debug.Log("No one die, Player Wins");
            ResetPlayerHP();
            RepositionUnits();
            isWin = true;

            shopManager.OpenShopOnWin();
            if (PSS != null)
            {
                SSS.GetAllShopSlotList();
                SSS.ShopoSaveData();
            }
            else
            {
                Debug.LogWarning("PlayerSaveSystem (PSS) is not assigned!");
            }
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
            if (attacker.CurrentEffects.Contains(DebuffEffectType.Slowness))
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
                    SoundManager.Instance.PlaySfxClipWithPitchChange(swordmanAttackAudio_);
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
                        SoundManager.Instance.PlaySfxClipWithPitchChange(bowmanAttackAudio_);
                        break;
                    }
                }
            }


            if (target != null)
            {
                int DamageTake = attacker.BasedATK + attacker.CurrentATK;
                if (target.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
                {
                    DamageTake *= 2;
                }
                else if (target.CurrentEffects.Contains(DebuffEffectType.Shield))
                {
                    DamageTake = 0;
                }
                else target.HP -= DamageTake;

                if (attacker.CurrentEffects.Contains(DebuffEffectType.LifeSteal))
                {
                    attacker.CurrentHP += DamageTake;
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
            if (attacker.CurrentEffects.Contains(DebuffEffectType.Slowness))
            {
                if (attacker.TurnSkipSlow == 0)
                {
                    attacker.TurnSkipSlow += 1;
                    continue;
                }
                else attacker.TurnSkipSlow = 0;
            }

            //EnemyCooldownDisplay - cooldown
            EnemyCooldownDisplay cdDisplay = attacker.GetComponent<EnemyCooldownDisplay>();
            if (cdDisplay != null)
            {
                cdDisplay.TickDownCooldown();
            }

            Vector3 originalPos = attacker.transform.position;
            Vector3 popPos = originalPos + Vector3.up * 100f;
            attacker.transform.position = popPos;

            yield return new WaitForSeconds(0.3f);

            totalATK += attacker.ATK;

            PlayerUnit targetPlayer = null;
            foreach (var unit in playerUnits)
            {
                if (!unit.isDead)
                {
                    targetPlayer = unit;
                    break;
                }
            }

            int PresumeDamageTake = attacker.ATK;
            if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
            {
                PresumeDamageTake *= 2;
            }
            else if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Shield))
            {
                PresumeDamageTake = 0;
            }
            if (attacker.CurrentEffects.Contains(DebuffEffectType.LifeSteal))
            {
                attacker.HP += PresumeDamageTake;
            }

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
            PlayerUnit targetPlayer = null;
            foreach (var unit in playerUnits)
            {
                if (!unit.isDead)
                {
                    targetPlayer = unit;
                    break;
                }
            }

            //EnemyCooldownDisplay - Target's Name
            foreach (var enemy in enemyUnits)
            {
                var display = enemy.GetComponent<EnemyCooldownDisplay>();
                if (display != null && targetPlayer != null)
                {
                    display.SetTargetText(targetPlayer.playerType.ToString());
                }
            }

            if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Vulnerable))
            {
                totalATK *= 2;
            }
            else if (targetPlayer.CurrentEffects.Contains(DebuffEffectType.Shield))
            {
                totalATK = 0;
            }
            else targetPlayer.CurrentHP -= totalATK;

            spawnAnimatedUI.EnemyAttackAnimationAt(0, 1);
            targetPlayer.UpdateUI();

            Debug.Log("Total Enemy ATK = " + totalATK);

            if (targetPlayer.BasedHP + targetPlayer.CurrentHP <= 0)
            {
                Debug.Log($"{targetPlayer.name} has died");
                targetPlayer.isDead = true;
                SetAlpha(targetPlayer.gameObject, 0.5f);

                //playerUnits.RemoveAt(0);
                //Destroy(targetPlayer.gameObject);
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

    private void SetAlpha(GameObject obj, float alpha)
    {
        var renderers = obj.GetComponentsInChildren<CanvasRenderer>();
        foreach (var r in renderers)
        {
            Color color = r.GetColor();
            color.a = alpha;
            r.SetColor(color);
        }
    }
    void Update()
    {
        PlayerUnit targetPlayer = playerUnits[0];
        if (targetPlayer.BasedHP + targetPlayer.CurrentHP <= 0)
        {
            Debug.Log($"{targetPlayer.name} has died");
            targetPlayer.isDead = true;
            SetAlpha(targetPlayer.gameObject, 0.5f);
            //playerUnits.RemoveAt(0);
            //Destroy(targetPlayer.gameObject);
            RepositionUnits();
        }
        PlayerUnit targetPlayer1 = playerUnits[1];
        if (targetPlayer.BasedHP + targetPlayer.CurrentHP <= 0)
        {
            Debug.Log($"{targetPlayer.name} has died");
            targetPlayer1.isDead = true;
            SetAlpha(targetPlayer1.gameObject, 0.5f);
            //playerUnits.RemoveAt(0);
            //Destroy(targetPlayer.gameObject);
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
