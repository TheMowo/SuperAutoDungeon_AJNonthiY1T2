using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TestingScripteAble : MonoBehaviour, IDataPersistence
{
    [Header("Player value")]
    public int HP = 10;
    public int Atk = 10;
    public string SpriteName = "test1_0";
    public Sprite sprite;

    [Header("Text show")]
    public Text textHP;
    public Text textAtk;
    public SpriteRenderer PlayerOBJ;

    [Header("Attributes SO")]
    [SerializeField] private SomeShit SOMETHING;


    void Update()
    {
        textHP.text = HP.ToString();
        textAtk.text = Atk.ToString();
        PlayerOBJ.sprite = sprite;
        SpriteName = sprite.name;
    }

    public void LoadData(GameData data)
    {
        SOMETHING.HP = data.playerAttributesData.HP;
        SOMETHING.ATK = data.playerAttributesData.ATK;
        SOMETHING.SpriteName = data.playerAttributesData.SpriteName;
        //SOMETHING.Sprite = Resources.Load<Sprite>(data.playerAttributesData.SpriteName);
        Debug.Log(SpriteName);
        Debug.Log($"Loaded Sprite Name: {SOMETHING.SpriteName}");
    }
    public void SaveData(ref GameData data)
    {
        data.playerAttributesData.HP = SOMETHING.HP;
        data.playerAttributesData.ATK = SOMETHING.ATK;
        data.playerAttributesData.SpriteName = SOMETHING.SpriteName;
        //data.playerAttributesData.sprite = Resources.Load < Sprite >(SOMETHING.SpriteName);
        Debug.Log($"Loaded Sprite Name: {data.playerAttributesData.SpriteName}");
    }
    
}
