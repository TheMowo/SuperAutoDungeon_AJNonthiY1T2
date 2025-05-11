using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesUnit : MonoBehaviour
{
    [SerializeField] public EnemiesUnitType enemiesUnitType;
    public int HP;
    public int ATK;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    
    int DropGold;
    int MPDrop;

    ConsumableItem FavFood;

    public List<DebuffEffectType> CurrentEffects;
    public int TurnSkipSlow;

    int CurrentGreyDebuff;
    int CurrentGreenDebuff;
    int CurrentLightBlueDebuff;
    int CurrentGoldDebuff;

    EnemiesUnitType.EnemiesType myType;


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
        HP = enemiesUnitType.HP;
        ATK = enemiesUnitType.ATK;
        UpdateVisual();
        myType = enemiesUnitType.myType;
        //SR = GetComponent<SpriteRenderer>();
        //SR.sprite = playerUnitType.Sprite;
    }

    public void UseConsumable(ConsumableItem consumable)
    {
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.Food)
        {
            if (consumable == FavFood)
            {
                OnFed();
            }
            HP += consumable.HpEffect;
            ATK += consumable.AtkEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.StatsPotion)
        {
            bool reverseEffect = false;
            if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion)
            {
                if (myType == EnemiesUnitType.EnemiesType.Undead)
                {
                    reverseEffect = true;
                }
            }
            if (consumable.GreyDebuffEffect != 0)
            {
                if (myType == EnemiesUnitType.EnemiesType.Strength)
                {
                    reverseEffect = true;
                }
            }
            if (consumable.LightBlueDebuffEffect != 0)
            {
                if (myType == EnemiesUnitType.EnemiesType.Fire)
                {
                    reverseEffect = true;
                }
            }
            if (consumable.GoldDebuffEffect != 0)
            {
                if (myType == EnemiesUnitType.EnemiesType.Steel)
                {
                    reverseEffect = true;
                }
            }
            if (reverseEffect)
            {
                HP -= consumable.HpEffect;
                ATK -= consumable.AtkEffect;
            }
            else 
            {
                HP += consumable.HpEffect;
                ATK += consumable.AtkEffect;
            }

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
        hpText.text = $"HP {HP}";
        atkText.text = $"ATK {ATK}";
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
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

    void UpdateVisual()
    {
        GreyDebuffBar.fillAmount = (float)CurrentGreyDebuff / 3;
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

    void OnFed()
    {
        Debug.Log("Fed");
    }
}
