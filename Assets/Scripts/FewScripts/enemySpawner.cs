using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign enemy prefab in Inspector
    public List<Transform> enemyPositions; // Drag E1, E2, E3 in Inspector

    public void SpawnEnemies(List<EnemiesUnit> enemyUnits)
    {
        if (enemyPrefab == null || enemyPositions.Count < 3)
        {
            Debug.LogWarning("Enemy Prefab missing or not enough spawn positions!");
            return;
        }

        int enemyCount = Random.Range(2, 4); // Spawn 2 or 3 enemies
        List<Transform> shuffledPositions = new List<Transform>(enemyPositions);

        ShuffleList(shuffledPositions); // Shuffle positions

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = shuffledPositions[i]; // Get unique position
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, GameObject.Find("UnitsSpriteHitbox").transform);
            EnemiesUnit enemyUnit = newEnemy.GetComponent<EnemiesUnit>();

            if (enemyUnit != null)
            {
                enemyUnits.Add(enemyUnit); // Add to CombatSystem's enemy list
            }
        }
    }

    private void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
