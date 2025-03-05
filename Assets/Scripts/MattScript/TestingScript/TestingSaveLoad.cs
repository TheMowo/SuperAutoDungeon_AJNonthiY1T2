using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TestingSaveLoad : MonoBehaviour, IDataPersistence
{
    public Text ScoreText;
    public int ScoreCount;

    public void LoadData(GameData data)
    {
        this.ScoreCount = data.ScoreCount;
    }
    public void SaveData(ref GameData data)
    {
        data.ScoreCount = this.ScoreCount;
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
