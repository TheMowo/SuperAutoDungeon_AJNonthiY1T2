using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Sword,
    Bow
}

public enum DebuffEffectType
{
    Poison,
    PoisonII,
    Weakness,
    WeaknessII,
    Vulnerable,
    Lethal,
    Slowness,
    Frozen,
}

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] public PlayerUnitType playerUnitType;
    public string uniqueID;
    public int BasedHP;
    public int BasedATK;
    public int CurrentHP;
    public int CurrentATK;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public Image unitSprite;
    public PlayerType playerType;
    public List<DebuffEffectType> CurrentEffects;
    public int TurnSkipSlow;

    int CurrentGreyDebuff;
    int CurrentGreenDebuff;
    int CurrentLightBlueDebuff;
    int CurrentGoldDebuff;


    [SerializeField] Image GreyDebuffBar;
    [SerializeField] Image GreenDebuffBar;
    [SerializeField] Image LightBlueDebuffBar;
    [SerializeField] Image GoldDebuffBar;

    [SerializeField] TMP_Text GreyDebuffText;
    [SerializeField] TMP_Text GreenDebuffText;
    [SerializeField] TMP_Text LightBlueDebuffText;
    [SerializeField] TMP_Text GoldDebuffText;



    [SerializeField] int MaxDebuff = 3;

    private void Awake()
    {
        BasedHP = playerUnitType.HP;
        BasedATK = playerUnitType.ATK;
        
        unitSprite.sprite = playerUnitType.UnitSprite;
        UpdateVisual();
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UseConsumable(ConsumableItem consumable)
    {
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.Food)
        {
            BasedHP += consumable.HpEffect;
            BasedATK += consumable.AtkEffect;
            CurrentHP += consumable.HpEffect;
            CurrentATK += consumable.AtkEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.StatsPotion)
        {
            CurrentHP += consumable.HpEffect;
            CurrentATK += consumable.AtkEffect;

            CurrentGreyDebuff += consumable.GreyDebuffEffect;

            CurrentGreenDebuff += consumable.GreenDebuffEffect;

            CurrentLightBlueDebuff += consumable.LightBlueDebuffEffect;

            CurrentGoldDebuff += consumable.GoldDebuffEffect;


            if (CurrentGreyDebuff < 0)
                CurrentGreyDebuff = 0;
            if (CurrentGreenDebuff < 0)
                CurrentGreenDebuff = 0;
            if (CurrentLightBlueDebuff < 0)
                CurrentLightBlueDebuff = 0;
            if (CurrentGoldDebuff < 0)
                CurrentGoldDebuff = 0;

            CheckDebuffFull();
            CheckDebuffFull();
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.CleansingPotion)
        {
            CurrentGreyDebuff = 0;
            CurrentGreenDebuff = 0;
            CurrentLightBlueDebuff = 0;
            CurrentGoldDebuff = 0;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.BetterCleansingPotion)
        {
            CurrentGreyDebuff = 0;
            CurrentGreenDebuff = 0;
            CurrentLightBlueDebuff = 0;
            CurrentGoldDebuff = 0;

            CurrentEffects.Clear();
        }
        UpdateVisual();
    }

    void CheckDebuffFull()
    {
        if (CurrentGreenDebuff >= 3)
        {
            ApplyDebuffEffects(DebuffEffectType.Poison);
            CurrentGreenDebuff -= 3;
        }
        if (CurrentGreyDebuff >= 3)
        {
            ApplyDebuffEffects(DebuffEffectType.Weakness);
            CurrentGreyDebuff -= 3;
        }
        if (CurrentLightBlueDebuff >= 3)
        {
            ApplyDebuffEffects(DebuffEffectType.Slowness);
            CurrentLightBlueDebuff -= 3;
        }
        if (CurrentGoldDebuff >= 3)
        {
            ApplyDebuffEffects(DebuffEffectType.Vulnerable);
            CurrentGoldDebuff -= 3;
        }
    }
    public void UpdateUI()
    {
        hpText.text = $"HP {BasedHP}";
        atkText.text = $"ATK {BasedATK}";
    }
    public void TakeDamage(int damage)
    {
        BasedHP -= damage;
        UpdateUI();
    }

    void ApplyDebuffEffects(DebuffEffectType effect)
    {
        if (effect == DebuffEffectType.Poison)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Poison))
                CurrentEffects.Add(DebuffEffectType.Poison);
            else
                CurrentEffects.Add(DebuffEffectType.PoisonII);
        }
        if (effect == DebuffEffectType.Weakness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Weakness))
                CurrentEffects.Add(DebuffEffectType.Weakness);
            else
                CurrentEffects.Add(DebuffEffectType.WeaknessII);
        }
        if (effect == DebuffEffectType.Vulnerable)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Vulnerable))
                CurrentEffects.Add(DebuffEffectType.Vulnerable);
            else
                CurrentEffects.Add(DebuffEffectType.Lethal);
        }
        if (effect == DebuffEffectType.Slowness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Slowness))
                CurrentEffects.Add(DebuffEffectType.Slowness);
            else
                CurrentEffects.Add(DebuffEffectType.Frozen);
        }
    }

    public void UpdateVisual()
    {
        GreyDebuffBar.fillAmount = (float)CurrentGreyDebuff/ 3;
        GreenDebuffBar.fillAmount = (float)CurrentGreenDebuff / 3;
        LightBlueDebuffBar.fillAmount = (float)CurrentLightBlueDebuff / 3;
        GoldDebuffBar.fillAmount = (float)CurrentGoldDebuff / 3;
        if (CurrentEffects.Count == 0)
        {
            GreyDebuffText.text = "";
            GreenDebuffText.text = "";
            LightBlueDebuffText.text = "";
            GoldDebuffText.text = "";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Poison))
        {
            GreenDebuffText.text = "I";
        }
        if (CurrentEffects.Contains(DebuffEffectType.PoisonII))
        {
            GreenDebuffText.text = "II";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Weakness))
        {
            GreyDebuffText.text = "I";
        }
        if (CurrentEffects.Contains(DebuffEffectType.WeaknessII))
        {
            GreyDebuffText.text = "II";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Vulnerable))
        {
            GoldDebuffText.text = "I";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Lethal))
        {
            GoldDebuffText.text = "II";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Slowness))
        {
            LightBlueDebuffText.text = "I";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Frozen))
        {
            LightBlueDebuffText.text = "II";
        }
    }


    public PlayerSaveData GetSaveData()
    {
        //Debug.Log("Data Save : it did save Huh?");
        return new PlayerSaveData
        {
            uniqueID = this.uniqueID,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            BaseHP = this.BasedHP,
            CurrentHP = this.CurrentHP,
            BaseATK = this.BasedATK,
            CurrentATK = this.BasedATK,
            playerType = this.playerType,
            CurrentGreyDebuff = this.CurrentGreyDebuff,
            CurrentGreenDebuff = this.CurrentGreenDebuff,
            CurrentLightBlueDebuff = this.CurrentLightBlueDebuff,
            CurrentGoldDebuff = this.CurrentGoldDebuff,
            CurrentEffects = this.CurrentEffects,
        };
    }

    public void LoadFromSaveData(PlayerSaveData data)
    {
        //Debug.Log("Data Load : it did Load Huh?");
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        this.BasedHP = data.BaseHP;
        this.CurrentHP = data.CurrentHP;
        this.BasedATK = data.BaseATK;
        this.CurrentATK = data.CurrentATK;
        this.playerType = data.playerType;
        this.CurrentGreyDebuff = data.CurrentGreyDebuff;
        this.CurrentGreenDebuff = data.CurrentGreenDebuff;
        this.CurrentLightBlueDebuff = data.CurrentLightBlueDebuff;
        this.CurrentGoldDebuff = data.CurrentGoldDebuff;
        this.CurrentEffects = data.CurrentEffects;
    }
}
