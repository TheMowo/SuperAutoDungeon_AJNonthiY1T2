using TMPro;
using UnityEngine;

public class ATKText : MonoBehaviour
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
            text.text = $"ATK  {hitbox.playerUnit.BasedATK}";
        }
        else { text.text = $"ATK  {hitbox.enemiesUnit.ATK}"; }
    }
}
