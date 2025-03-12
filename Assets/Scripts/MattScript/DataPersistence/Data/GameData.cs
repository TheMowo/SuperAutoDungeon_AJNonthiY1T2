using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Game Variable Save")]
    public int Currency;

    public AttributesData playerAttributesData;
    public AttributesDataEnemy enemyAttributesData;

    public GameData()
    {
        this.Currency = 0;
        this.playerAttributesData = new AttributesData();
        this.enemyAttributesData = new AttributesDataEnemy();
    }
}
