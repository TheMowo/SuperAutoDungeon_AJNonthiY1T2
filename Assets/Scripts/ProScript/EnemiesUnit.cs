using UnityEngine;

public class EnemiesUnit : MonoBehaviour
{
    [SerializeField] EnemiesUnitType enemiesUnitType;
    public int HP;
    public int ATK;
    int DropGold;
    int MPDrop;
    SpriteRenderer SR;
    ConsumableItem FavFood;

    int CurrentRedDebuff;
    int CurrentGreenDebuff;
    int CurrentBlueDebuff;

    [SerializeField] int FullRedDebuff = 5;
    [SerializeField] int FullGreenDebuff = 6;
    [SerializeField] int FullBlueDebuff = 7;

    private void Awake()
    {
        HP = enemiesUnitType.HP;
        ATK = enemiesUnitType.ATK;
        FavFood = enemiesUnitType.FavFood;
        DropGold = enemiesUnitType.DropGold;
        MPDrop = enemiesUnitType.MPDrop;
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = enemiesUnitType.Sprite;
    }

    public void UseConsumable(ConsumableItem consumable)
    {
        if (FavFood == consumable)
        {
            OnFed();
            return;
        }
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

    void OnDead()
    {

    }

    void OnFed()
    {
        Debug.Log("Fed");
    }
}
