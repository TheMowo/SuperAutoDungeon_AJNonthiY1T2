using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Game Variable Save")]
    public int ScoreCount;

    public AttributesData playerAttributesData;
    public AttributesDataEnemy enemyAttributesData;

    public GameData()
    {
        this.ScoreCount = 0;
        this.playerAttributesData = new AttributesData();
        this.enemyAttributesData = new AttributesDataEnemy();
    }
}
