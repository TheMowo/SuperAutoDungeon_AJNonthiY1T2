using System.Collections.Generic;
using TMPro;
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
    Weakness,
    Vulnerable,
    Slowness,
    Shield,
    LifeSteal
}

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] public PlayerUnitType playerUnitType;
    public string uniqueID;
    public bool isDead;
    public int BasedHP;
    public int BasedATK;
    public int CurrentHP;
    public int CurrentATK;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public Image playerUnitSprite;
    public PlayerType playerType;
    public List<DebuffEffectType> CurrentEffects;
    public int TurnSkipSlow;

    public int GreyDebuffDuration;
    public int GreenDebuffDuration;
    public int LightBlueDebuffDuration;
    public int GoldDebuffDuration;
    public int LifeStealDebuffDuration;
    public int ShieldDebuffDuration;


    [SerializeField] Image GreyDebuffBar;
    [SerializeField] Image GreenDebuffBar;
    [SerializeField] Image LightBlueDebuffBar;
    [SerializeField] Image GoldDebuffBar;
    [SerializeField] Image ShieldDebuffBar; 
    [SerializeField] Image LifeStealDebuffBar;

    [SerializeField] Image GreyDebuffBox;
    [SerializeField] Image GreenDebuffBox;
    [SerializeField] Image LightBlueDebuffBox;
    [SerializeField] Image GoldDebuffBox;
    [SerializeField] Image ShieldDebuffBox;
    [SerializeField] Image LifeStealDebuffBox;



    [SerializeField] TMP_Text GreyDebuffText;
    [SerializeField] TMP_Text GreenDebuffText;
    [SerializeField] TMP_Text LightBlueDebuffText;
    [SerializeField] TMP_Text GoldDebuffText;
    [SerializeField] TMP_Text ShieldDebuffText;
    [SerializeField] TMP_Text LifeStealDebuffText;



    [SerializeField] int MaxDebuff = 5;

    private void Awake()
    {
        BasedHP = playerUnitType.HP;
        BasedATK = playerUnitType.ATK;
        
        playerUnitSprite.sprite = playerUnitType.UnitSprite;
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
            //CurrentHP += consumable.HpEffect;
            //CurrentATK += consumable.AtkEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.StatsPotion)
        {
            
            CurrentHP += consumable.HpEffect;
            CurrentATK += consumable.AtkEffect;

            GreyDebuffDuration += consumable.GreyDebuffEffect;

            GreenDebuffDuration += consumable.GreenDebuffEffect;

            LightBlueDebuffDuration += consumable.LightBlueDebuffEffect;

            GoldDebuffDuration += consumable.GoldDebuffEffect;

            if (GreyDebuffDuration > 5)
            {
                GreyDebuffDuration = 5;
            }
            if (GreenDebuffDuration > 5)
            {
                GreenDebuffDuration = 5;
            }
            if (GoldDebuffDuration > 5)
            {
                GoldDebuffDuration = 5;
            }
            if (LightBlueDebuffDuration > 5)
            {
                LightBlueDebuffDuration = 5;
            }
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.InstantDamage)
        {
            CurrentHP += consumable.HpEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.Shield)
        {
            CurrentHP += consumable.HpEffect;
            CurrentATK += consumable.AtkEffect;
            ShieldDebuffDuration += consumable.ShieldDebuffEffect;
            if (ShieldDebuffDuration > 5)
            {
                ShieldDebuffDuration = 5;
            }
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.LifeStealPotion)
        {
            CurrentHP += consumable.HpEffect;
            CurrentATK += consumable.AtkEffect;
            LifeStealDebuffDuration += consumable.LifeStealDebuffEffect;
            if (LifeStealDebuffDuration > 5)
            {
                LifeStealDebuffDuration = 5;
            }
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.FragileStrengthPotion)
        {
            BasedHP  /= 2;
            CurrentHP /= 2;
            BasedATK += BasedHP + CurrentHP;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.CleansingPotion)
        {
            GreyDebuffDuration = 0;
            GreenDebuffDuration = 0;
            LightBlueDebuffDuration = 0;
            GoldDebuffDuration = 0;

            CurrentEffects.Clear();
        }

        CheckDebuff();
    }

    public void CheckDebuff()
    {
        if (GreenDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.Poison);
        }
        else CurrentEffects.Remove(DebuffEffectType.Poison);
        if (GreyDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.Weakness);
        }
        else CurrentEffects.Remove(DebuffEffectType.Weakness);
        if (LightBlueDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.Slowness);
        }
        else CurrentEffects.Remove(DebuffEffectType.Slowness);
        if (GoldDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.Vulnerable);
        }
        else CurrentEffects.Remove(DebuffEffectType.Vulnerable);
        if (ShieldDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.Shield);
        }
        else CurrentEffects.Remove(DebuffEffectType.Shield);
        if (LifeStealDebuffDuration > 0)
        {
            ApplyDebuffEffects(DebuffEffectType.LifeSteal);
        }
        else CurrentEffects.Remove(DebuffEffectType.LifeSteal);

        UpdateVisual();
    }
    public void UpdateUI()
    {
        hpText.text = $"HP {BasedHP}";
        atkText.text = $"ATK {BasedATK}";
    }

    bool ApplyDebuffEffects(DebuffEffectType effect)
    {
        bool canApply = false;
        if (effect == DebuffEffectType.Poison)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Poison))
            {
                CurrentEffects.Add(DebuffEffectType.Poison);
                canApply = true;
            }
            else
                canApply = false;
        }
        if (effect == DebuffEffectType.Weakness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Weakness))
            {
                CurrentEffects.Add(DebuffEffectType.Weakness);
                canApply = true;
            }
            else
                canApply = false;
        }
        if (effect == DebuffEffectType.Vulnerable)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Vulnerable))
            {
                CurrentEffects.Add(DebuffEffectType.Vulnerable);
                canApply = true;
            }
            else
                canApply = false;
        }
        if (effect == DebuffEffectType.Slowness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Slowness))
            {
                CurrentEffects.Add(DebuffEffectType.Slowness);
                canApply = true;
            }
            else
                canApply = false;
        }
        if (effect == DebuffEffectType.Shield)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Shield))
            {
                CurrentEffects.Add(DebuffEffectType.Shield);
                canApply = true;
            }
            else
                canApply = false;
        }
        if (effect == DebuffEffectType.LifeSteal)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.LifeSteal))
            {
                CurrentEffects.Add(DebuffEffectType.LifeSteal);
                canApply = true;
            }
            else
                canApply = false;
        }
        return canApply;
    }

    public void UpdateVisual()
    {
        if (GreyDebuffDuration != 0)
        {
            GreyDebuffBox.gameObject.SetActive(true);
        }
        else GreyDebuffBox.gameObject.SetActive(false);
        if (GreenDebuffDuration != 0)
        {
            GreenDebuffBox.gameObject.SetActive(true);
        }
        else GreenDebuffBox.gameObject.SetActive(false);
        if (GoldDebuffDuration != 0)
        {
            GoldDebuffBox.gameObject.SetActive(true);
        }
        else GoldDebuffBox.gameObject.SetActive(false);
        if (LightBlueDebuffDuration != 0)
        {
            LightBlueDebuffBox.gameObject.SetActive(true);
        }
        else LightBlueDebuffBox.gameObject.SetActive(false);
        if (LifeStealDebuffDuration != 0)
        {
            LifeStealDebuffBox.gameObject.SetActive(true);
        }
        else LifeStealDebuffBox.gameObject.SetActive(false);
        if (ShieldDebuffDuration != 0)
        {
            ShieldDebuffBox.gameObject.SetActive(true);
        }
        else ShieldDebuffBox.gameObject.SetActive(false);

        TooltipTrigger[] tooltipTriggers = GetComponentsInChildren<TooltipTrigger>();
        List<GameObject> debuffBars = new List<GameObject>();

        foreach (TooltipTrigger trigger in tooltipTriggers)
        {
            debuffBars.Add(trigger.gameObject);
        }

        float StartingPos = (debuffBars.Count - 1) * - 22.5f;

        foreach (GameObject debuff in debuffBars)
        {
            debuff.GetComponent<RectTransform>().localPosition = new Vector3(StartingPos, 0);
            StartingPos += 45f;
        }

        GreyDebuffBar.fillAmount = (float)GreyDebuffDuration/ MaxDebuff;
        GreenDebuffBar.fillAmount = (float)GreenDebuffDuration / MaxDebuff;
        LightBlueDebuffBar.fillAmount = (float)LightBlueDebuffDuration / MaxDebuff;
        GoldDebuffBar.fillAmount = (float)GoldDebuffDuration / MaxDebuff;
        ShieldDebuffBar.fillAmount = (float)ShieldDebuffDuration / MaxDebuff;
        LifeStealDebuffBar.fillAmount = (float)LifeStealDebuffDuration / MaxDebuff;

        if (CurrentEffects.Count == 0)
        {
            GreyDebuffText.text = "";
            GreenDebuffText.text = "";
            LightBlueDebuffText.text = "";
            GoldDebuffText.text = "";
            ShieldDebuffText.text = "";
            LifeStealDebuffText.text = "";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Poison))
        {
            GreenDebuffText.text = $"{GreenDebuffDuration}";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Weakness))
        {
            GreyDebuffText.text = $"{GreyDebuffDuration}";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Vulnerable))
        {
            GoldDebuffText.text = $"{GoldDebuffDuration}";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Slowness))
        {
            LightBlueDebuffText.text = $"{LightBlueDebuffDuration}";
        }
        if (CurrentEffects.Contains(DebuffEffectType.Shield))
        {
            ShieldDebuffText.text = $"{ShieldDebuffDuration}";
        }
        if (CurrentEffects.Contains(DebuffEffectType.LifeSteal))
        {
            LifeStealDebuffText.text = $"{LifeStealDebuffDuration}";
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
            CurrentGreyDebuff = this.GreyDebuffDuration,
            CurrentGreenDebuff = this.GreenDebuffDuration,
            CurrentLightBlueDebuff = this.LightBlueDebuffDuration,
            CurrentGoldDebuff = this.GoldDebuffDuration,
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
        this.GreyDebuffDuration = data.CurrentGreyDebuff;
        this.GreenDebuffDuration = data.CurrentGreenDebuff;
        this.LightBlueDebuffDuration = data.CurrentLightBlueDebuff;
        this.GoldDebuffDuration = data.CurrentGoldDebuff;
        this.CurrentEffects = data.CurrentEffects;
    }
}
