using System;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public enum PlayerType
{
    Sword,
    Bow
}

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] public PlayerUnitType playerUnitType;
    public string uniqueID;
    public int HP;
    public int ATK;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public PlayerType playerType;

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

    public PlayerSaveData GetSaveData()
    {
        //Debug.Log("Data Save : it did save Huh?");
        return new PlayerSaveData
        {
            uniqueID = this.uniqueID,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            Health = this.HP,
            Attack = this.ATK,
            playerType = this.playerType
        };
    }

    public void LoadFromSaveData(PlayerSaveData data)
    {
        //Debug.Log("Data Load : it did Load Huh?");
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        this.HP = data.Health;
        this.ATK = data.Attack;
        this.playerType = data.playerType;
    }
}
