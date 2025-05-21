using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesUnit : MonoBehaviour
{
    [SerializeField] public EnemiesUnitType enemiesUnitType;
    public string uniqueID;
    public int HP;
    public int ATK;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public Image enemiesUnitSprite;

    int DropGold;
    int MPDrop;

    ConsumableItem FavFood;

    public List<DebuffEffectType> CurrentEffects;
    public int TurnSkipSlow;

    EnemiesUnitType.EnemiesType myType;


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
        Debug.Log("test awake enemy unit 1");
        HP = enemiesUnitType.HP;
        ATK = enemiesUnitType.ATK;
        Debug.Log("test awake enemy unit 2");
        enemiesUnitSprite.sprite = enemiesUnitType.UnitSprite;
        Debug.Log("test awake enemy unit 3");
        UpdateVisual();
        myType = enemiesUnitType.myType;
    }

    private void Start()
    {
        UpdateVisual();
    }

    public void UseConsumable(ConsumableItem consumable)
    {
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.Food)
        {
            HP += consumable.HpEffect;
            ATK += consumable.AtkEffect;
            //CurrentHP += consumable.HpEffect;
            //CurrentATK += consumable.AtkEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.StatsPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.InstantDamage)
        {
            bool reverseEffect = false;
            if (consumable.myEffectType == ConsumableItem.ItemEffectType.HealingPotion || consumable.myEffectType == ConsumableItem.ItemEffectType.InstantDamage)
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
            HP += consumable.HpEffect;
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.Shield)
        {
            HP += consumable.HpEffect;
            ATK += consumable.AtkEffect;
            ShieldDebuffDuration += consumable.ShieldDebuffEffect;
            if (ShieldDebuffDuration > 5)
            {
                ShieldDebuffDuration = 5;
            }
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.LifeStealPotion)
        {
            HP += consumable.HpEffect;
            ATK += consumable.AtkEffect;
            LifeStealDebuffDuration += consumable.LifeStealDebuffEffect;
            if (LifeStealDebuffDuration > 5)
            {
                LifeStealDebuffDuration = 5;
            }
        }
        if (consumable.myEffectType == ConsumableItem.ItemEffectType.FragileStrengthPotion)
        {
            HP /= 2;
            ATK += HP;
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
            {
                CurrentEffects.Add(DebuffEffectType.Poison);
            }
        }
        if (effect == DebuffEffectType.Weakness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Weakness))
            {
                CurrentEffects.Add(DebuffEffectType.Weakness);
            }
        }
        if (effect == DebuffEffectType.Vulnerable)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Vulnerable))
            {
                CurrentEffects.Add(DebuffEffectType.Vulnerable);
            }
        }
        if (effect == DebuffEffectType.Slowness)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Slowness))
            {
                CurrentEffects.Add(DebuffEffectType.Slowness);
            }
        }
        if (effect == DebuffEffectType.Shield)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.Shield))
            {
                CurrentEffects.Add(DebuffEffectType.Shield);
            }
        }
        if (effect == DebuffEffectType.LifeSteal)
        {
            if (!CurrentEffects.Contains(DebuffEffectType.LifeSteal))
            {
                CurrentEffects.Add(DebuffEffectType.LifeSteal);
            }
        }
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

        float StartingPos = (debuffBars.Count - 1) * -22.5f;

        foreach (GameObject debuff in debuffBars)
        {
            debuff.GetComponent<RectTransform>().localPosition = new Vector3(StartingPos, 0);
            StartingPos += 45f;
        }

        GreyDebuffBar.fillAmount = (float)GreyDebuffDuration / MaxDebuff;
        GreenDebuffBar.fillAmount = (float)GreenDebuffDuration / MaxDebuff;
        LightBlueDebuffBar.fillAmount = (float)LightBlueDebuffDuration / MaxDebuff;
        GoldDebuffBar.fillAmount = (float)GoldDebuffDuration / MaxDebuff;
        ShieldDebuffBar.fillAmount = (float)ShieldDebuffDuration / MaxDebuff;
        LifeStealDebuffBar.fillAmount = (float)LifeStealDebuffDuration / MaxDebuff ;
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

    void OnFed()
    {
        Debug.Log("Fed");
    }

    public EnemySaveSystem ESS;

    public EnemySaveData GetSaveData()
    {
        Debug.Log("Data Save : it did save Huh?");
        if (FindObjectsByType<EnemiesUnit>(FindObjectsSortMode.None).ToList() != null)
        {
            return new EnemySaveData
            {
                uniqueID = this.uniqueID,
                position = new float[] { transform.position.x, transform.position.y, transform.position.z },
                BaseHP = this.HP,
                BaseATK = this.ATK,
                CurrentGreyDebuff = this.GreyDebuffDuration,
                CurrentGreenDebuff = this.GreenDebuffDuration,
                CurrentLightBlueDebuff = this.LightBlueDebuffDuration,
                CurrentGoldDebuff = this.GoldDebuffDuration,
                CurrentEffects = this.CurrentEffects,
                LifeStealDebuff = this.LifeStealDebuffDuration,
                ShieldDebuff = this.ShieldDebuffDuration,
            };
        }
        else
        {
            return null;
        }
    }

    public void LoadFromSaveData(EnemySaveData data)
    {
        //Debug.Log("Data Load : it did Load Huh?");
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        this.ATK = data.BaseATK;
        this.HP = data.BaseHP;
        this.GreyDebuffDuration = data.CurrentGreyDebuff;
        this.GreenDebuffDuration = data.CurrentGreenDebuff;
        this.LightBlueDebuffDuration = data.CurrentLightBlueDebuff;
        this.GoldDebuffDuration = data.CurrentGoldDebuff;
        this.CurrentEffects = data.CurrentEffects;
        this.LifeStealDebuffDuration = data.LifeStealDebuff;
        this.ShieldDebuffDuration = data.ShieldDebuff;
    }

}
