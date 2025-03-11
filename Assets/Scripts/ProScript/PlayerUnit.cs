using TMPro;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] public PlayerUnitType playerUnitType;
    public int HP;
    public int ATK;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;

    int CurrentRedDebuff;
    int CurrentGreenDebuff;
    int CurrentBlueDebuff;

    [SerializeField] int FullRedDebuff = 5;
    [SerializeField] int FullGreenDebuff = 6;
    [SerializeField] int FullBlueDebuff = 7;

    private void Awake()
    {
        HP = playerUnitType.HP;
        ATK = playerUnitType.ATK;
        //SR = GetComponent<SpriteRenderer>();
        //SR.sprite = playerUnitType.Sprite;
    }

    public void UseConsumable(ConsumableItem consumable)
    {
        HP += consumable.HpEffect;
        ATK += consumable.AtkEffect;
        CurrentRedDebuff += consumable.RedDebuffEffect;
        CurrentGreenDebuff += consumable.GreenDebuffEffect;
        CurrentBlueDebuff += consumable.BlueDebuffEffect;
        CheckDebuffFull();
    }

    void CheckDebuffFull()
    {
        if (CurrentRedDebuff >= FullRedDebuff)
        {
            CurrentRedDebuff -= FullRedDebuff;
            Debug.Log("Trigger Red Side Effects");
        }
        if (CurrentGreenDebuff >= FullGreenDebuff)
        {
            CurrentGreenDebuff -= FullGreenDebuff;
            Debug.Log("Trigger Green Side Effects");
        }
        if (CurrentBlueDebuff >= FullBlueDebuff)
        {
            CurrentBlueDebuff -= FullBlueDebuff;
            Debug.Log("Trigger Blue Side Effects");
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
}
