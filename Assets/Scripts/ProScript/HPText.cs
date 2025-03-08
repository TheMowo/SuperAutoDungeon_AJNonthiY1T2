using UnityEngine;
using TMPro;

public class HPText : MonoBehaviour
{
    TMP_Text text;
    UnitHitbox hitbox;
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        hitbox = GetComponentInParent<UnitHitbox>();
    }

    private void Update()
    {
        if (hitbox.isPlayerUnit)
        {
            text.text = $"HP  {hitbox.playerUnit.HP}";
        }
        else { text.text = $"HP  {hitbox.enemiesUnit.HP}"; }
    }
}
