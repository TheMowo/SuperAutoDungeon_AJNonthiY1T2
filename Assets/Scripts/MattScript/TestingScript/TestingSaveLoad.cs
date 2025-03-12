using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TestingSaveLoad : MonoBehaviour, IDataPersistence
{
    public Text ScoreText;
    public int ScoreCount;

    public void LoadData(GameData data)
    {
        this.ScoreCount = data.Currency;
    }
    public void SaveData(ref GameData data)
    {
        data.Currency = this.ScoreCount;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ScoreCount += 1;
        }

        ScoreText.text = "" + ScoreCount;
    }
}
