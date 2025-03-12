using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TestingScripteAble : MonoBehaviour, IDataPersistence
{
    [Header("Player value")]
    public int HP = 10;
    public int ATK = 10;
    public string SpriteName = "test1_0";
    public Sprite sprite;

    [Header("Text show")]
    public Text textHP;
    public Text textAtk;
    public Image PlayerOBJ;

    [Header("Attributes SO")]
    [SerializeField] private SomeShit SOMETHING;


    void Update()
    {
         sprite = PlayerOBJ.sprite;
        //SpriteName = sprite.name;
    }

    public void LoadData(GameData data)
    {
        SOMETHING.HP = data.playerAttributesData.HP;
        SOMETHING.ATK = data.playerAttributesData.ATK;
        SOMETHING.SpriteName = data.playerAttributesData.SpriteName;

        if (!string.IsNullOrEmpty(SOMETHING.SpriteName))
        {
          Sprite  loading_sprite = Resources.Load<Sprite>("Assest/Sprites/" + SpriteName);
            if (sprite != null)
            {
                PlayerOBJ.sprite = loading_sprite;
                Debug.Log($"Loaded Sprite: {SpriteName}");
            }
            else
            {
                Debug.LogWarning($"Sprite '{SpriteName}' not found in Resources!");
            }
        } 
        
        textHP.text = HP.ToString();
        textAtk.text = ATK.ToString();
        PlayerOBJ.sprite = sprite;
    }
    public void SaveData(ref GameData data)
    {
        data.playerAttributesData.HP = SOMETHING.HP;
        data.playerAttributesData.ATK = SOMETHING.ATK;

        // Save sprite name
        if (sprite != null)
        {
            data.playerAttributesData.SpriteName = sprite.name;
            data.playerAttributesData.sprite = sprite;// Save only the name
            Debug.Log($"Saved Sprite Name: {sprite.name}");
        }
        else
        {
            Debug.LogWarning("Sprite is null, cannot save!");
        }
    }
    
}
