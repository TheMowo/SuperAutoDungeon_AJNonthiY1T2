using UnityEngine;

public class Unit
{
    private int _hp;
    private int _atk;

    public int HP { get { return _hp; } }
    public int ATK { get { return _atk; } }

    public void SetHP(int hp)
    {
        _hp = hp;
    }

    public void SetATK(int atk)
    {
        _atk = atk;
    }
}
