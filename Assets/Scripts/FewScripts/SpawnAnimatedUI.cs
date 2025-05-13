using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnAnimatedUI : MonoBehaviour
{
    public List<GameObject> animatedUIPrefabs;
    public RectTransform UICanva;
    public CombatSystem combatSystem;

    public int targetIndex = 0;

    [System.Obsolete]
    void Awake()
    {
        if (combatSystem == null)
        {
            combatSystem = FindObjectOfType<CombatSystem>();
        }
    }
    public void PlayAnimationAt(int index, int animIndex = 0)
    {
        if (index < 0 || index >= combatSystem.enemyUnits.Count) return;
        if (animIndex < 0 || animIndex >= animatedUIPrefabs.Count) return;
        
        RectTransform enemyRect = combatSystem.enemyUnits[index].GetComponent<RectTransform>();
        Vector2 enemyPos = enemyRect.anchoredPosition;

        GameObject anim = Instantiate(animatedUIPrefabs[animIndex], UICanva);
        RectTransform rect = anim.GetComponent<RectTransform>();
        rect.anchoredPosition = enemyPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnimationAt(targetIndex);
        }
    }
}
